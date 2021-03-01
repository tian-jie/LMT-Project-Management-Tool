using Microsoft.AspNetCore.Mvc;
using Microsoft.eShopWeb.BusinessCore.Interfaces;
using Microsoft.eShopWeb.Web.Controllers.Api;
using System.Linq;
using System.Threading.Tasks;

namespace Microsoft.eShopWeb.BackendAdmin.Controllers
{
    public class ResourcePlanApiController : BaseApiController
    {
        private readonly IResourcePlanService _resourcePlanService;

        public ResourcePlanApiController(IResourcePlanService resourcePlanService)
        {
            _resourcePlanService = resourcePlanService;
        }


        //public async Task<IActionResult> ListInternal()
        //{
        //    var roles = await _roleTitleService.GetAllInternalRoles();

        //    return new JsonResult(new
        //    {
        //        code = 200,
        //        Message = $"success, {roles.Count()} records.",
        //        data = roles
        //    });
        //}

        //public async Task<IActionResult> ListExternal()
        //{
        //    var roles = await _roleTitleService.GetAllExternalRoles();

        //    return new JsonResult(new
        //    {
        //        code = 200,
        //        Message = $"success, {roles.Count()} records.",
        //        data = roles
        //    });
        //}


    }
}
