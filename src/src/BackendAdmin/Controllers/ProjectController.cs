using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Microsoft.eShopWeb.BackendAdmin.Controllers
{
    [Authorize]
    public class ProjectController : Controller
    {
        public ProjectController()
        {
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Detail()
        {
            return View();
        }



    }
}
