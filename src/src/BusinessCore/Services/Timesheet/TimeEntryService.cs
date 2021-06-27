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
        private readonly IProjectService _projectService;
        private readonly IProjectTaskService _projectTaskService;
        private readonly IEmployeeService _employeeService;
        private readonly IUserGroupService _userGroupService;

        public TimeEntryService(CatalogContext dbContext
            , IRoleTitleService roleTitleService
            , IEffortUsedByRoleByDateService acByRoleByDateService
            , IEmployeeTitleService employeeTitleService
            , IUserGroupService userGroupService
            , IProjectService projectService
            , IProjectTaskService projectTaskService
            , IEmployeeService employeeService
            )
            : base(dbContext)
        {
            _roleTitleService = roleTitleService;
            _acByRoleByDateService = acByRoleByDateService;
            _employeeTitleService = employeeTitleService;
            _userGroupService = userGroupService;
            _projectService = projectService;
            _projectTaskService = projectTaskService;
            _employeeService = employeeService;
        }

        public async Task<List<EffortUsedByRoleByDate>> CalculateACByRole(string projectGid, string taskGid, DateTime startDate, DateTime endDate, List<TimeEntry> timeEntries = null)
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


        /// <summary>
        /// 根据给定的GroupId和指定的时间段，获取TimeEntry
        /// </summary>
        /// <param name="groupId">groupId，如果为空，则不过滤这个字段，全部查询</param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        public async Task<List<TimeEntry>> GetTimeEntriesByGroupAndDurationAsync(string groupId, DateTime startDate, DateTime endDate)
        {
            List<TimeEntry> timeEntries;
            if (!string.IsNullOrEmpty(groupId))
            {
                var userIds = (await _userGroupService.WhereAsync(a => a.IsDeleted != true && a.GroupId == groupId)).Select(a => a.UserId).ToList();
                timeEntries = (await WhereAsync(a => a.IsDeleted != true && userIds.Contains(a.UserId) && a.Date >= startDate && a.Date < endDate)).ToList();
            }
            else
            {
                timeEntries = (await WhereAsync(a => a.IsDeleted != true && a.Date >= startDate && a.Date < endDate)).ToList();
            }

            return timeEntries;
        }


        public async Task<List<TimeEntriesGroupByEmployeeView>> GetTimeEntriesByProjectGroupByEmployee(string projectGid)
        {
            // 获取项目相关的员工
            // var projectUsers = _projectUserService.GetUserByProject(projectGid, null);
            var allUsers = await _employeeService.AllEmployeesWithRole();
            var roleTitles = await _roleTitleService.GetAllInternalRoles();


            // 看这个projectGid，如果属于taskgid，就从taskgid过滤
            //var task = _projectTaskService.GetProjectTaskById(projectGid);

            // 再根据每个员工统计timeentry
            //var timeEntry = Repository.Entities.Where(a => a.IsDeleted != true);
            //if (task != null && task.Count > 0)
            //{
            //    timeEntry = timeEntry.Where(a => a.TaskId == projectGid);
            //}
            //else
            //{
            //    timeEntry = timeEntry.Where(a => a.ProjectId == projectGid);
            //}

            //var sql = @"
            //    select * from TimeEntry TE where 
            //    exists(select 1 from Project P where P.Gid = TE.projectId and Gid = '{0}')
            //    or exists (select 1 from TaskToProjectMapping TPM where TPM.TaskGid = TE.TaskId and TPM.ProjectGid='{0}')
            //    order by [date]";

            var timeEntry = (await WhereAsync(a=>a.ProjectId == projectGid)).ToList();
            //var timeEntry = Repository.SqlQuery(string.Format(sql, projectGid));
            var timeEntryByUsers = timeEntry.GroupBy(a => a.UserId);
            var timeEntriesGroupByEmployeesView = new List<TimeEntriesGroupByEmployeeView>();

            foreach (var a in timeEntryByUsers)
            {
                var employee = allUsers.FirstOrDefault(b => b.Gid == a.Key);

                var tv = new TimeEntriesGroupByEmployeeView()
                {
                    UserId = a.Key,
                    TotalHours = a.Sum(b => b.TotalHours),
                    EmployeeRate = employee.RoleRate,
                    EmployeeRole = employee.RoleName,
                    TotalHoursRate = a.Sum(b => b.TotalHours) * employee.RoleRate,
                    EmployeeName = employee.Name
                };

                tv.TotalEffortByWeek = new List<TotalEffortByWeek>();

                // 计算周数
                foreach (var v in a)
                {
                    // 计算这个日期属于第几周
                    var week = v.Date.WeekOfYear();
                    var year = v.Date.YearOfWeekOfYear();

                    var totalEffortByWeek = tv.TotalEffortByWeek.FirstOrDefault(b => b.WeekNumber == week);
                    if (totalEffortByWeek == null)
                    {
                        tv.TotalEffortByWeek.Add(new TotalEffortByWeek()
                        {
                            Year = year,
                            WeekNumber = week,
                            TotalHours = v.TotalHours,
                            TotalHoursRate = v.TotalHours * employee.RoleRate
                        });
                    }
                    else
                    {
                        totalEffortByWeek.TotalHours += v.TotalHours;
                        totalEffortByWeek.TotalHoursRate += v.TotalHours * employee.RoleRate;
                    }
                }

                timeEntriesGroupByEmployeesView.Add(tv);
            }
            return timeEntriesGroupByEmployeesView;
        }

        public async Task<ProjectAccountingView> GetTimeEntriesByProject(string projectGid)
        {
            var timeEntriesGroupByEmployeesView = await GetTimeEntriesByProjectGroupByEmployee(projectGid);

            var projectInfo = await _projectService.GetProjectById(projectGid);

            var projectAccountingView = new ProjectAccountingView()
            {
                ProjectGid = projectGid,
                ProjectId = projectInfo.Id,
                ProjectName = projectInfo.Name,
                SpentManHour = timeEntriesGroupByEmployeesView.Sum(a => a.TotalHours),
                SpentManHourRate = timeEntriesGroupByEmployeesView.Sum(a => a.TotalHoursRate)
            };

            return projectAccountingView;
        }

        public async Task<List<TimesheetByWeekView>> GetTimeEntriesByEmployeeGroupByProject(string employeeId, DateTime startDate, DateTime endDate)
        {
            var timeEntries = (await WhereAsync(a => a.IsDeleted != true && a.Date >= startDate && a.Date < endDate && a.UserId == employeeId)).ToList();
            var userTimeEntriesByUser = timeEntries.GroupBy(a => new { a.ProjectId, a.TaskId });

            var projects = await _projectService.GetAllProjects();
            var tasks = await _projectTaskService.GetAllProjectTasks();

            var timesheetByWeekViews = new List<TimesheetByWeekView>();
            // 把上面整理好的每个人每天的信息，整理到按周几放到的TimesheetByWeekView里。
            foreach (var ud in userTimeEntriesByUser)
            {
                var project = projects.FirstOrDefault(a => a.Gid == ud.Key.ProjectId);
                var task = tasks.FirstOrDefault(a => a.Gid == ud.Key.TaskId);
                var tv = new TimesheetByWeekView()
                {
                    ProjectName = project == null ? "N/A Project" : project.Name,
                    TaskName = task == null ? "/-" : " / " + task.Name,
                    ProjectGid = ud.Key.ProjectId,
                    TaskGid = ud.Key.TaskId
                };

                foreach (var udd in ud.GroupBy(a => a.Date))
                {
                    var totalHoursByDate = udd.Sum(a => a.TotalHours);

                    var weekOfDay = udd.Key.DayOfWeek;
                    switch (weekOfDay)
                    {
                        case DayOfWeek.Monday:
                            tv.MondayTotalHours = totalHoursByDate;
                            break;
                        case DayOfWeek.Tuesday:
                            tv.TuesdayTotalHours = totalHoursByDate;
                            break;
                        case DayOfWeek.Wednesday:
                            tv.WednesdayTotalHours = totalHoursByDate;
                            break;
                        case DayOfWeek.Thursday:
                            tv.ThursdayTotalHours = totalHoursByDate;
                            break;
                        case DayOfWeek.Friday:
                            tv.FridayTotalHours = totalHoursByDate;
                            break;
                        case DayOfWeek.Saturday:
                            tv.SaturdayTotalHours = totalHoursByDate;
                            break;
                        case DayOfWeek.Sunday:
                            tv.SundayTotalHours = totalHoursByDate;
                            break;
                    }

                }
                timesheetByWeekViews.Add(tv);
            }

            return timesheetByWeekViews;
        }

        public async Task<List<EmployeeView>> GetEmployeeByProjectTimeEntry(string projectGid)
        {
            //var employeeIds = Repository.Entities.Where(a => a.IsDeleted != true && a.ProjectId == projectGid).Select(a => a.UserId).Distinct();
//            var sql = @"select * from TimeEntry TE
//where exists(select 1 from TaskToProjectMapping TPM where TPM.ProjectGid = TE.ProjectId and TPM.ProjectGId = '{0}')
//or exists(select 1 from TaskToProjectMapping TPM where TPM.TaskGid = TE.TaskId and TPM.ProjectGId = '{0}')";

            var employeeIds = (await WhereAsync(a => a.IsDeleted == false && a.ProjectId == projectGid)).Select(a=>a.UserId).Distinct().ToList();
            
            
            //var employeeIds = Repository.SqlQuery(string.Format(sql, projectGid)).Select(a => a.UserId).Distinct();

            var employees = new List<EmployeeView>();
            var allEmployees = (await _employeeService.WhereAsync(a => a.IsDeleted != true)).ToList();
            foreach (var employeeId in employeeIds)
            {
                var employee = allEmployees.FirstOrDefault(a => a.Gid == employeeId);
                var employeeView = new EmployeeView()
                {
                    Id = employee.Id,
                    Gid = employee.Gid,
                    Name = employee.DisplayName,
                    DefaultWorkspace = employee.DefaultWorkspace,
                    Email = employee.Email,
                    ProfilePicture = employee.ProfilePicture,
                    Status = employee.Status
                };

                employees.Add(employeeView);
            }

            return employees;
        }


        public async Task<List<EffortByWeekView>> GetActualEffortByWeek(string projectGid, DateTime date)
        {
            var year = date.YearOfWeekOfYear();
            var week = date.WeekOfYear();

            //var sql = @"
            //    select * from TimeEntry TE where 
            //    (exists(select 1 from Project P where P.Gid = TE.projectId and Gid = '{0}')
            //    or exists (select 1 from TaskToProjectMapping TPM where TPM.TaskGid = TE.TaskId and TPM.ProjectGid='{0}')) and Date<'{1}'
            //    order by [date]";

            //var timeEntries = Repository.SqlQuery(string.Format(sql, projectGid, date.ToString("yyyy-MM-dd")));

            var timeEntries = (await WhereAsync(a => a.ProjectId == projectGid && a.Date < date)).ToList();

            // 找到第一周和最后一周
            var firstWeekData = timeEntries.OrderBy(a => a.Date).FirstOrDefault();
            var lastWeekData = timeEntries.OrderByDescending(a => a.Date).FirstOrDefault();
            var lastWeek = lastWeekData.Date.YearOfWeekOfYear() * 100 + lastWeekData.Date.WeekOfYear();
            // 获取rate数据
            var employeesWithRole = await _employeeService.AllEmployeesWithRole();

            var effortsByWeek = new List<EffortByWeekView>();
            // 创建所有周的数据
            var y = firstWeekData.Date.YearOfWeekOfYear();
            var w = firstWeekData.Date.WeekOfYear();
            while (y * 100 + w <= lastWeek)
            {
                effortsByWeek.Add(new EffortByWeekView()
                {
                    Year = y,
                    Week = w
                });
                w++;
                if (w > 53)
                {
                    w = 1;
                    y++;
                }
            }

            foreach (var te in timeEntries)
            {
                var teYear = te.Date.YearOfWeekOfYear();
                var teWeek = te.Date.WeekOfYear();

                var effortByWeekView = effortsByWeek.FirstOrDefault(a => a.Year == teYear && a.Week == teWeek);
                effortByWeekView.TotalHours += te.TotalHours;
                effortByWeekView.TotalHoursRate += te.TotalHours * employeesWithRole.FirstOrDefault(a => a.Gid == te.UserId).RoleRate;
            }

            return effortsByWeek;
        }




    }
}
