using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Microsoft.eShopWeb.BackendAdmin.Controllers
{
    [Authorize]
    public class ResourcePlanController : Controller
    {
        public ResourcePlanController()
        {
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

    }
}
