using Microsoft.AspNetCore.Mvc;
using Microsoft.eShopWeb.BusinessCore.Interfaces;
using Microsoft.eShopWeb.Web.Controllers.Api;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Microsoft.eShopWeb.BackendAdmin.Controllers
{
    public class ProjectAccountingApiController : BaseApiController
    {
        private readonly ITimeEntryService _timeEntryService;
        private readonly IProjectTaskService _projectTaskService;

        public ProjectAccountingApiController(IProjectTaskService projectTaskService,
            ITimeEntryService timeEntryService)
        {
            _projectTaskService = projectTaskService;
            _timeEntryService = timeEntryService;
        }


        public async Task<IActionResult> CalculateAc(string startDate, string endDate)
        {
            // by项目进行计算
            var tasks = await _projectTaskService.ListAllAsync();

            foreach (var task in tasks)
            {
                await _timeEntryService.CalculateACByRoleAndSave(task.ProjectGid, task.Gid, DateTime.Parse(startDate), DateTime.Parse(endDate));

            }

            return new JsonResult(new
            {
                Message = $"Calculate finished! - {tasks.Count()} tasks!"
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
