using Microsoft.AspNetCore.Mvc;

namespace Microsoft.eShopWeb.BackendAdmin.Controllers
{
    public class EmployeeController : Controller
    {

        public EmployeeController()
        {
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

    }
}
