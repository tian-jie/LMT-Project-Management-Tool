using Microsoft.eShopWeb.ApplicationCore.Entities;
using Microsoft.eShopWeb.BusinessCore.Interfaces;
using Microsoft.eShopWeb.BusinessCore.ViewModel;
using Microsoft.eShopWeb.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Microsoft.eShopWeb.BusinessCore.Services
{
    public class TimeEntryService : BaseService<TimeEntry>, ITimeEntryService
    {
        private readonly IRoleTitleService _roleTitleService;
        private readonly IEffortUsedByRoleByDateService _acByRoleByDateService;
        private readonly IEmployeeTitleService _employeeTitleService;

        public TimeEntryService(CatalogContext dbContext,
            IRoleTitleService roleTitleService,
            IEffortUsedByRoleByDateService acByRoleByDateService,
            IEmployeeTitleService employeeTitleService)
            : base(dbContext)
        {
            _roleTitleService = roleTitleService;
            _acByRoleByDateService = acByRoleByDateService;
            _employeeTitleService = employeeTitleService;
        }

        public async Task<List<EffortUsedByRoleByDate>> CalculateACByRoleAndSave(string projectGid, string taskGid, DateTime startDate, DateTime endDate, List<TimeEntry> timeEntries = null)
        {
            if (timeEntries == null)
            {
                // 如果没有传的话，就取一遍
                var timeEntriesQ1 = await WhereAsync(a => a.ProjectId == projectGid && a.Date >= startDate && a.Date <= endDate && a.IsDeleted == false);
                if (!string.IsNullOrEmpty(taskGid))
                {
                    timeEntriesQ1 = timeEntriesQ1.Where(a => a.TaskId == taskGid);
                }

                timeEntries = timeEntriesQ1.ToList();
            }


            var timeEntriesQ = timeEntries.Where(a => a.ProjectId == projectGid);
            if (!string.IsNullOrEmpty(taskGid))
            {
                timeEntriesQ = timeEntriesQ.Where(a => a.TaskId == taskGid);
            }

            if (timeEntriesQ.Count() == 0)
            {
                return new List<EffortUsedByRoleByDate>();
            }

            var rolesByMonth = new Dictionary<string, List<EmployeeTitle>>();
            var timeEntriesByMonth = new Dictionary<string, List<TimeEntryWithRoleViewModel>>();
            var sdate = startDate.AddDays(-startDate.Day + 1);
            var edate = endDate.AddDays(-endDate.Day + 1);

            for (var month = sdate; month <= edate; month = month.AddMonths(1))
            {
                var roleByMonth = await _employeeTitleService.ListByMonth(month.Year, month.Month);
                rolesByMonth.Add(month.ToString("yyyyMM"), roleByMonth);

                var timeEntriesByTaskByMonth = from timeEntry in timeEntriesQ.Where(a => a.Date >= month && a.Date < month.AddMonths(1))
                                               join role in roleByMonth on timeEntry.UserId equals role.EmployeeGid
                                               select new TimeEntryWithRoleViewModel()
                                               {
                                                   Gid = timeEntry.Gid,
                                                   Description = timeEntry.Description,
                                                   Date = timeEntry.Date,
                                                   ProjectId = projectGid,
                                                   TaskId = taskGid,
                                                   RoleId = role.RoleId,
                                                   IsBillable = timeEntry.IsBillable,
                                                   TotalHours = timeEntry.TotalHours,
                                                   UserId = timeEntry.UserId
                                               };

                if (timeEntriesByTaskByMonth.Count() == 0)
                {
                    continue;
                }

                timeEntriesByMonth.Add(month.ToString("yyyyMM"), timeEntriesByTaskByMonth.ToList());
            }

            var effortUsed = new List<EffortUsedByRoleByDate>();

            for (var date = startDate; date <= endDate; date = date.AddDays(1))
            {
                var month = date.AddDays(-date.Day + 1);

                var timeEntriesByRoleByMonth = timeEntriesByMonth.GetValueOrDefault(month.ToString("yyyyMM"));
                if (timeEntriesByRoleByMonth == null || timeEntriesByRoleByMonth.Count() == 0)
                {
                    continue;
                }
                effortUsed.AddRange(await CalculateACByDateByRoleAndSave(projectGid, taskGid, date, timeEntriesByRoleByMonth));
            }

            return effortUsed;
        }


        private async Task<List<EffortUsedByRoleByDate>> CalculateACByDateByRoleAndSave(string projectGid, string taskGid, DateTime date, List<TimeEntryWithRoleViewModel> timeEntriesByTask = null)
        {
            // TODO: 如果未传入timeEntriesByTask，则要从数据库里取一下

            var acByRoles = await CalculateACByDateByRole(projectGid, taskGid, date, timeEntriesByTask);

            // 删除当天的统计数据，然后再插入新的
            // 不在这里删除了，到外面统一删除
            //await _acByRoleByDateService.SqlExecuteNonQuery($"delete from [AcByRoleByDate] where [date]='{date.ToString("yyyy-MM-dd")}'");

            //var acsByRoleByDate = new List<EffortUsedByRoleByDate>();
            //foreach (var c in acByRoles)
            //{
            //    acsByRoleByDate.Add(new EffortUsedByRoleByDate()
            //    {
            //        ProjectGid = projectGid,
            //        TaskGid = taskGid,
            //        Date = date,
            //        RoleId = c.RoleId,
            //        RoleName = c.RoleName,
            //        RoleRate = c.RoleRate,
            //        TotalHours = c.TotalHours,
            //        TotalHoursRate = c.TotalHoursRate
            //    });
            //}

            //await _acByRoleByDateService.AddManyAsync(acByRoles);
            return acByRoles;
        }


        private async Task<List<EffortUsedByRoleByDate>> CalculateACByDateByRole(string projectGid, string taskGid, DateTime date, List<TimeEntryWithRoleViewModel> timeEntriesByTask)
        {
            // 取出所有的role
            var roles = await _roleTitleService.GetAllInternalRoles();

            var acByRoles = new List<EffortUsedByRoleByDate>();
            //foreach (var role in roles)
            //{
            //    acByRoles.Add(new EffortUsedAccountingViewModel()
            //    {
            //        RoleId = role.Id,
            //        TotalHours = 0,
            //        TotalHoursRate = 0,
            //        RoleName = role.Title,
            //        RoleRate = role.Rate
            //    });
            //}

            var timeEntriesByRole = timeEntriesByTask.Where(a => a.Date.Date == date.Date).GroupBy(a => a.RoleId);

            foreach (var r in timeEntriesByRole)
            {
                var roleId = r.Key;
                var role = roles.FirstOrDefault(a => a.Id == roleId);
                if (role == null)
                {
                    role = new RoleTitle()
                    {
                        Id = 0,
                        Rate = 100,
                        Title = "Default Title",
                        Category = "Others Category",
                        ShortTitle = "Default",
                        Type = "internal",
                        Sort = 1000
                    };
                }
                var totalHours = r.Sum(a => a.TotalHours);
                var totalHoursRate = totalHours * role.Rate;

                var acRole = acByRoles.FirstOrDefault(a => a.RoleId == roleId);
                if (acRole == null)
                {
                    acRole = new EffortUsedByRoleByDate()
                    {
                        RoleId = role.Id,
                        TotalHours = 0,
                        TotalHoursRate = 0,
                        RoleName = role.Title,
                        RoleRate = role.Rate,
                        Date = date,
                        ProjectGid = projectGid,
                        TaskGid = taskGid,
                        RoleCategory = role.Category,
                        RoleShortTitle = role.ShortTitle
                    };
                    acByRoles.Add(acRole);
                }
                acRole.TotalHours = totalHours;
                acRole.TotalHoursRate = totalHoursRate;
            }

            return acByRoles;
        }

        public async Task<List<EffortUsedAccountingViewModel>> CalculateACByWeekByRole(string projectGid, string taskGid, DateTime date, List<TimeEntryWithRoleViewModel> timeEntriesByTask)
        {
            // 取出所有的role
            var roles = await _roleTitleService.GetAllInternalRoles();

            var acByRoles = new List<EffortUsedAccountingViewModel>();
            foreach (var role in roles)
            {
                acByRoles.Add(new EffortUsedAccountingViewModel()
                {
                    RoleId = role.Id,
                    TotalHours = 0,
                    TotalHoursRate = 0,
                    RoleName = role.Title,
                    RoleRate = role.Rate
                });
            }

            var timeEntriesByRole = timeEntriesByTask.Where(a => a.Date == date).GroupBy(a => a.RoleId);

            foreach (var r in timeEntriesByRole)
            {
                var roleId = r.Key;
                var role = roles.FirstOrDefault(a => a.Id == roleId);
                var totalHours = r.Sum(a => a.TotalHours);
                var totalHoursRate = totalHours * role.Rate;

                var acRole = acByRoles.FirstOrDefault(a => a.RoleId == roleId);
                acRole.TotalHours = totalHours;
                acRole.TotalHoursRate = totalHoursRate;
            }

            return acByRoles;
        }


        public async Task<List<ProjectTask>> GetAvaliableTaskByTimeEntry(DateTime startDate, DateTime endDate)
        {
            var timeEntriesQ = await WhereAsync(a => a.Date >= startDate && a.Date <= endDate && a.IsDeleted == false);
            var tasks = timeEntriesQ.Select(a => new ProjectTask()
            {
                ProjectGid = a.ProjectId,
                Gid = a.TaskId
            }).Distinct().ToList();
            return tasks.ToList();
        }

    }
}
