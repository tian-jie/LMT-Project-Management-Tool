using Microsoft.AspNetCore.Mvc;

namespace Microsoft.eShopWeb.Web.Controllers
{
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

    }
}
