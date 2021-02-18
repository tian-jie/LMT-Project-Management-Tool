using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.eShopWeb.BackendAdmin.Interfaces;
using Microsoft.eShopWeb.BackendAdmin.ServiceInterfaces;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Microsoft.eShopWeb.Web.Controllers
{
    [ApiExplorerSettings(IgnoreApi = true)]
    [Authorize] // Controllers that mainly require Authorization still use Controller/View; other pages use Pages
    [Route("[controller]/[action]")]
    public class HomeController : Controller
    {
        private readonly IClockifyService _clockifyService;
        private readonly ITimesheetService _timesheetService;
        private readonly IEmployeeService _employeeService;
        private readonly IEmployeeTitleService _employeeTitleService;
        private readonly ISharePointUserService _sharePointUserService;

        ApplicationPartManager _applicationPartManager;

        public HomeController(IClockifyService clockifyService,
            ITimesheetService timesheetService,
            ApplicationPartManager applicationPartManager,
            IEmployeeService employeeService,
            IEmployeeTitleService employeeTitleService,
            ISharePointUserService sharePointUserService)
        {
            _clockifyService = clockifyService;
            _timesheetService = timesheetService;
            _applicationPartManager = applicationPartManager;
            _employeeService = employeeService;
            _employeeTitleService = employeeTitleService;
            _sharePointUserService = sharePointUserService;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult SharepointUsers()
        {
            return View();
        }

        [HttpPost]
        public JsonResult SyncUsersAsync()
        {
            return Task.Run(async () =>
            {

                var login = await _clockifyService.Login("jie.tian@innocellence.com", "Welcome1!");
                var workspaceId = login.membership.FirstOrDefault(a => a.membershipType == "WORKSPACE").targetId;

                var cnt = await _timesheetService.SyncUsers(workspaceId, login.token);
                return Json(new
                {
                    Message = $"Success Synced: {cnt}"
                });
            }).Result;

        }

        [HttpPost]
        public async Task<JsonResult> SyncProjectsAsync()
        {
            var login = await _clockifyService.Login("jie.tian@innocellence.com", "Welcome1!");
            var workspaceId = login.membership.FirstOrDefault(a => a.membershipType == "WORKSPACE").targetId;
            var cnt = await _timesheetService.SyncProjects(workspaceId, login.token);
            return Json(new
            {
                Message = $"Success Synced: {cnt}"
            });
        }

        [HttpPost]
        public async Task<IActionResult> SyncGroups()
        {
            var login = await _clockifyService.Login("jie.tian@innocellence.com", "Welcome1!");
            var workspaceId = login.membership.FirstOrDefault(a => a.membershipType == "WORKSPACE").targetId;
            var cnt = await _timesheetService.SyncGroups(workspaceId, login.token);
            return Json(new
            {
                Message = $"Success Synced: {cnt}"
            });
        }

        [HttpPost]
        public async Task<IActionResult> SyncClients()
        {
            var login = await _clockifyService.Login("jie.tian@innocellence.com", "Welcome1!");
            var workspaceId = login.membership.FirstOrDefault(a => a.membershipType == "WORKSPACE").targetId;
            var cnt = await _timesheetService.SyncClients(workspaceId, login.token);
            return Json(new
            {
                Message = $"Success Synced: {cnt}"
            });
        }

        [HttpPost]
        public async Task<IActionResult> SyncTimeEntryV3(string startTime, string endTime)
        {
            var login = await _clockifyService.Login("jie.tian@innocellence.com", "Welcome1!");
            var workspaceId = login.membership.FirstOrDefault(a => a.membershipType == "WORKSPACE").targetId;

            var sdt = DateTime.ParseExact(startTime, "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture);
            var edt = DateTime.ParseExact(endTime, "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture);

            var cnt = await _timesheetService.SyncTimeRecordsV3(workspaceId, login.token, sdt, edt);
            return Json(new
            {
                Message = $"Success Synced: {cnt}"
            });
        }

        [HttpPost]
        public async Task<IActionResult> SyncTimeEntryV2(string startTime, string endTime)
        {
            var login = await _clockifyService.Login("jie.tian@innocellence.com", "Welcome1!");
            var workspaceId = login.membership.FirstOrDefault(a => a.membershipType == "WORKSPACE").targetId;

            var sdt = DateTime.ParseExact(startTime, "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture);
            var edt = DateTime.ParseExact(endTime, "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture);

            var cnt = await _timesheetService.SyncTimeRecordsV2(workspaceId, login.token, sdt, edt);
            return Json(new
            {
                Message = $"Success Synced: {cnt}"
            });
        }

        public IActionResult Demo()
        {
            //var viewModel = await _mediator.Send(new GetMyOrders(User.Identity.Name));

            return View();
        }

    }
}
