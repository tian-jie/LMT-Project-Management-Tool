using Microsoft.AspNetCore.Mvc;
using Microsoft.eShopWeb.ApplicationCore.Entities;
using Microsoft.eShopWeb.ApplicationCore.Interfaces;
using Microsoft.eShopWeb.BusinessCore.Interfaces;
using Microsoft.eShopWeb.Web.Controllers.Api;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Microsoft.eShopWeb.BackendAdmin.Controllers
{
    public class ProjectAccountingApiController : BaseApiController
    {
        private readonly ITimeEntryService _timeEntryService;
        private readonly IProjectTaskService _projectTaskService;
        private readonly IEffortUsedByRoleByDateService _acByRoleByDateService;
        private readonly IAppLogger<ProjectAccountingApiController> _logger;

        public ProjectAccountingApiController(IProjectTaskService projectTaskService,
            IEffortUsedByRoleByDateService acByRoleByDateService,
            ITimeEntryService timeEntryService,
            IAppLogger<ProjectAccountingApiController> logger)
        {
            _projectTaskService = projectTaskService;
            _timeEntryService = timeEntryService;
            _acByRoleByDateService = acByRoleByDateService;
            _logger = logger;
        }


        public async Task<IActionResult> CalculateAc(string startDate, string endDate)
        {
            // by项目进行计算
            var tasks = await _timeEntryService.GetAvaliableTaskByTimeEntry(DateTime.Parse(startDate), DateTime.Parse(endDate));
            var effortUsed = new List<EffortUsedByRoleByDate>();

            var timeEntries = (await _timeEntryService.WhereAsync(a => a.Date >= DateTime.Parse(startDate) && a.Date < DateTime.Parse(endDate).AddDays(1))).ToList();

            var index = 0;
            var totalCnt = tasks.Count();
            foreach (var task in tasks)
            {
                effortUsed.AddRange(await _timeEntryService.CalculateACByRoleAndSave(task.ProjectGid, task.Gid, DateTime.Parse(startDate), DateTime.Parse(endDate), timeEntries));
                _logger.LogInformation($"progress: {index++}/{totalCnt}, {(index*100/totalCnt).ToString("F2")}%");
            }

            await _acByRoleByDateService.SqlExecuteNonQuery($"delete from [AcByRoleByDate] where [date]>='{startDate}' and [date]<='{endDate}'");

            await _acByRoleByDateService.AddManyAsync(effortUsed);

            return new JsonResult(new
            {
                Message = $"Calculate finished! - {tasks.Count()} tasks!, EffortUsed - {effortUsed.Count()} records."
            });
        }

        public async Task<IActionResult> CalculateAcByTask(string startDate, string endDate, string projectGid, string taskGid)
        {
            await _timeEntryService.CalculateACByRoleAndSave(projectGid, taskGid, DateTime.Parse(startDate), DateTime.Parse(endDate));

            return new JsonResult(new
            {
                Message = $"Calculate finished!"
            });
        }


    }
}
