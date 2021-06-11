using Microsoft.AspNetCore.Mvc;

namespace Microsoft.eShopWeb.BackendAdmin.Controllers
{
    public class MenuController : Controller
    {
        public MenuController()
        {
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }
    }
}