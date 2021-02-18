using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.eShopWeb.ApplicationCore.Entities;
using Microsoft.eShopWeb.BackendAdmin.Interfaces;
using Microsoft.eShopWeb.BackendAdmin.ServiceInterfaces;
using Microsoft.eShopWeb.BackendAdmin.ViewModels;
using Microsoft.eShopWeb.Web.Controllers.Api;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Microsoft.eShopWeb.BackendAdmin.Controllers
{
    public class ProjectApiController : BaseApiController
    {
        private readonly ITimeEntryService _timeEntryService;
        private readonly IProjectTaskService _projectTaskService;
        private readonly IProjectService _projectService;
        private readonly IProjectOwnerService _projectOwnerService;
        private readonly IEffortUsedByRoleByDateService _effortUsedByRoleByDateService;

        public ProjectApiController(IProjectService projectService,
            ITimeEntryService timeEntryService,
            IProjectTaskService projectTaskService,
            IEffortUsedByRoleByDateService effortUsedByRoleByDateService,
            IProjectOwnerService projectOwnerService)
        {
            _projectTaskService = projectTaskService;
            _timeEntryService = timeEntryService;
            _projectService = projectService;
            _effortUsedByRoleByDateService = effortUsedByRoleByDateService;
            _projectOwnerService = projectOwnerService;
        }


        public async Task<IActionResult> EffortUsedByDay(string projectGid, string taskGid)
        {
            var effortsByDay = await _effortUsedByRoleByDateService.GetEffortUsedByDay(projectGid, taskGid);

            // 数字取回来以后，需要处理一下，哪天有，哪天没有，就乱了。要从第一天到最后一天，然后累加

            effortsByDay = effortsByDay.OrderBy(a => a.Date).ToList();
            var firstDay = effortsByDay[0].Date;
            var lastDay = effortsByDay[effortsByDay.Count - 1].Date;

            var result = new List<EffortUsedViewModel>();
            decimal sumOfTotalHours = 0;
            decimal sumOfTotalHoursRate = 0;
            for (var day = firstDay; day <= lastDay; day = day.AddDays(1))
            {
                var effort = effortsByDay.FirstOrDefault(a => a.Date == day);
                if (effort != null)
                {
                    result.Add(effort);
                    effort.TotalHours += sumOfTotalHours;
                    effort.TotalHoursRate += sumOfTotalHoursRate;

                    sumOfTotalHours = effort.TotalHours;
                    sumOfTotalHoursRate = effort.TotalHoursRate;
                }
                else
                {
                    result.Add(new EffortUsedViewModel()
                    {
                        Date = day,
                        TotalHours = sumOfTotalHours,
                        TotalHoursRate = sumOfTotalHoursRate
                    });
                }
            }

            return new JsonResult(new
            {
                code = 200,
                Message = $"success, {effortsByDay.Count()} records.",
                data = result
            });
        }

        public async Task<IActionResult> EffortUsedByRole(string projectGid, string taskGid)
        {
            var effortUsedByRole = (await _effortUsedByRoleByDateService.GetEffortUsedByRole(projectGid, taskGid));

            return new JsonResult(new
            {
                code = 200,
                Message = $"success, {effortUsedByRole.Count()} records.",
                data = effortUsedByRole
            });
        }

        public async Task<IActionResult> EffortUsedByRoleCategory(string projectGid, string taskGid)
        {
            var effortUsedByRole = (await _effortUsedByRoleByDateService.GetEffortUsedByRoleCategory(projectGid, taskGid));

            return new JsonResult(new
            {
                code = 200,
                Message = $"success, {effortUsedByRole.Count()} records.",
                data = effortUsedByRole
            });
        }

        public async Task<IActionResult> List(string favorateByUserId, string filterByTeam, string filterByOwner)
        {
            var result = await _projectService.ListAllAsync();
            if (string.IsNullOrEmpty(favorateByUserId))
            {

            }

            if (string.IsNullOrEmpty(filterByTeam))
            {

            }

            if (string.IsNullOrEmpty(filterByOwner))
            {
                var projectIdsByOwner = (await _projectOwnerService.WhereAsync(a => a.EmployeeGid==filterByOwner)).Select(a=>a.ProjectGid).ToList();
                result = result.Where(a => projectIdsByOwner.Contains(a.Gid)).ToList();
            }


            return new JsonResult(new
            {
                code = 200,
                total = result.Count,
                rows = result
            });
        }

        public async Task<IActionResult> ListWithStatus(string favorateByUserId, string filterByTeam, string filterByManager)
        {
            var result = await _projectService.ListAllAsync();

            // TODO: 增加显示status，EAC等数据
            return new JsonResult(new
            {
                code = 200,
                total = result.Count,
                rows = result
            });
        }

    }
}
