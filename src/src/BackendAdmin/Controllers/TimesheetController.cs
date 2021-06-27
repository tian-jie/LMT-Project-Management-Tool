using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.eShopWeb.ApplicationCore.Interfaces;
using Microsoft.eShopWeb.BusinessCore.Interfaces;
using System.Linq;
using System.Threading.Tasks;

namespace Microsoft.eShopWeb.BackendAdmin.Controllers
{
    [Authorize] // Controllers that mainly require Authorization still use Controller/View; other pages use Pages
    public class TimesheetController : Controller
    {
        private readonly IAppLogger<TimesheetController> _logger;
        private readonly ITimeEntryService _timeEntryService;
        private readonly ITimesheetService _timesheetService;
        private readonly IGroupService _groupService;
        private readonly IUserGroupService _userGroupService;
        private readonly IEmployeeService _employeeService;
        private readonly IProjectService _projectService;
        private readonly IYearWeekCalendarService _yearWeekCalendarService;

        public TimesheetController(ITimeEntryService timeEntryService
            , ITimesheetService timesheetService
            , IGroupService groupService, IUserGroupService userGroupService
            , IEmployeeService employeeService
            , IProjectService projectService
            , IYearWeekCalendarService yearWeekCalendarService)
        {
            _timeEntryService = timeEntryService;
            _timesheetService = timesheetService;
            _groupService = groupService;
            _userGroupService = userGroupService;
            _employeeService = employeeService;
            _projectService = projectService;
            _yearWeekCalendarService = yearWeekCalendarService;
        }

        [HttpGet]
        public async Task<ActionResult> Index()
        {
            var groups = (await _groupService.WhereAsync(a => a.IsDeleted != true)).OrderBy(a => a.Name);
            ViewBag.Groups = groups;
            var yearWeekCalendar = (await _yearWeekCalendarService.ListAllAsync()).ToList();
            ViewBag.YearWeekCalendar = yearWeekCalendar;
            return View();
        }

    }
}
