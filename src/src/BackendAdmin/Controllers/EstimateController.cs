using Microsoft.AspNetCore.Mvc;

namespace Microsoft.eShopWeb.BackendAdmin.Controllers
{
    public class EstimateController : Controller
    {

        public EstimateController()
        {
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

    }
}
