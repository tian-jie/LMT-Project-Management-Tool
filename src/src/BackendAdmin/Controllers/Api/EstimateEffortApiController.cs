using Ardalis.Specification;
using Microsoft.AspNetCore.Mvc;
using Microsoft.eShopWeb.ApplicationCore.Entities;
using Microsoft.eShopWeb.BackendAdmin.Interfaces;
using Microsoft.eShopWeb.Web.Controllers.Api;
using System.Linq;
using System.Threading.Tasks;

namespace Microsoft.eShopWeb.BackendAdmin.Controllers
{
    public class EstimateEffortApiController : BaseApiController
    {
        private readonly ITimeEntryService _timeEntryService;
        private readonly IProjectTaskService _projectTaskService;
        private readonly IProjectService _projectService;
        private readonly IEstimateEffortService _estimateEffortService;

        public EstimateEffortApiController(IProjectService projectService,
            ITimeEntryService timeEntryService,
            IProjectTaskService projectTaskService,
            IEstimateEffortService estimateEffortService)
        {
            _projectTaskService = projectTaskService;
            _timeEntryService = timeEntryService;
            _projectService = projectService;
            _estimateEffortService = estimateEffortService;
        }


        public async Task<IActionResult> GetListByProject(string projectGid)
        {
            var result = await _estimateEffortService.ListAsync(new EstimateEffortSpecification(projectGid));

            return new JsonResult(new
            {
                code = 200,
                total = result.Count,
                rows = result
            });

        }

        public async Task<IActionResult> Save(EstimateEffort estimate)
        {
            if (estimate.Id == 0)
            {
                await _estimateEffortService.AddAsync(estimate);
            }
            else
            {
                await _estimateEffortService.UpdateAsync(estimate);
            }

            return new JsonResult(new
            {
                code = 200,
                total = 1,
                message = estimate.Id == 0 ? "insert success" : "update success"
            });

        }


        public async Task<IActionResult> Delete(int id)
        {
            await _estimateEffortService.DeleteAsync(a => a.Id == id);

            return new JsonResult(new
            {
                code = 200,
                Message = $"success, deleted."
            });
        }

    }




    public class EstimateEffortSpecification : Specification<EstimateEffort>
    {
        public EstimateEffortSpecification(string projectGid)
        {
            Query.Where(a => a.ProjectGid == projectGid);
        }
    }
}
