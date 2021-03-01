using Microsoft.AspNetCore.Mvc;
using Microsoft.eShopWeb.ApplicationCore.Entities;
using Microsoft.eShopWeb.BusinessCore.Interfaces;
using Microsoft.eShopWeb.BusinessCore.ServiceInterfaces;
using Microsoft.eShopWeb.BusinessCore.ViewModel;
using Microsoft.eShopWeb.Web.Controllers.Api;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Microsoft.eShopWeb.BackendAdmin.Controllers
{
    public class ClockifyApiController : BaseApiController
    {
        private readonly IClockifyService _clockifyService;
        private readonly ITimesheetService _timesheetService;
        private readonly ISharePointUserService _sharePointUserService;

        public ClockifyApiController(IClockifyService clockifyService,
            ITimesheetService timesheetService,
            ISharePointUserService sharePointUserService)
        {
            _clockifyService = clockifyService;
            _timesheetService = timesheetService;
            _sharePointUserService = sharePointUserService;
        }

        public IActionResult TestSync()
        {
            return new JsonResult(new
            {
                Message = $"Success Synced: testSync - 1000"
            });

        }

        public IActionResult SyncUsers()
        {
            return Task.Run(async () =>
            {

                var login = await _clockifyService.Login("jie.tian@innocellence.com", "Welcome1!");
                var workspaceId = login.membership.FirstOrDefault(a => a.membershipType == "WORKSPACE").targetId;

                var cnt = await _timesheetService.SyncUsers(workspaceId, login.token);
                return new JsonResult(new
                {
                    Message = $"Success Synced: {cnt}"
                });
            }).Result;

        }

        public async Task<IActionResult> SyncProjects()
        {
            var login = await _clockifyService.Login("jie.tian@innocellence.com", "Welcome1!");
            var workspaceId = login.membership.FirstOrDefault(a => a.membershipType == "WORKSPACE").targetId;
            var cnt = await _timesheetService.SyncProjects(workspaceId, login.token);
            return new JsonResult(new
            {
                Message = $"Success Synced: {cnt}"
            });
        }

        public async Task<IActionResult> SyncGroups()
        {
            var login = await _clockifyService.Login("jie.tian@innocellence.com", "Welcome1!");
            var workspaceId = login.membership.FirstOrDefault(a => a.membershipType == "WORKSPACE").targetId;
            var cnt = await _timesheetService.SyncGroups(workspaceId, login.token);
            return new JsonResult(new
            {
                Message = $"Success Synced: {cnt}"
            });
        }

        public async Task<IActionResult> SyncClients()
        {
            var login = await _clockifyService.Login("jie.tian@innocellence.com", "Welcome1!");
            var workspaceId = login.membership.FirstOrDefault(a => a.membershipType == "WORKSPACE").targetId;
            var cnt = await _timesheetService.SyncClients(workspaceId, login.token);
            return new JsonResult(new
            {
                Message = $"Success Synced: {cnt}"
            });
        }


        public async Task<IActionResult> SyncTimeEntryV3(string startTime, string endTime)
        {
            var login = await _clockifyService.Login("jie.tian@innocellence.com", "Welcome1!");
            var workspaceId = login.membership.FirstOrDefault(a => a.membershipType == "WORKSPACE").targetId;

            var sdt = DateTime.ParseExact(startTime, "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture);
            var edt = DateTime.ParseExact(endTime, "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture);

            var cnt = await _timesheetService.SyncTimeRecordsV3(workspaceId, login.token, sdt, edt);
            return new JsonResult(new
            {
                Message = $"Success Synced: {cnt}"
            });
        }

        //[Route("api/v2/clockify/sync-time-entry")]
        public async Task<IActionResult> SyncTimeEntryV2(string startTime, string endTime)
        {
            var login = await _clockifyService.Login("jie.tian@innocellence.com", "Welcome1!");
            var workspaceId = login.membership.FirstOrDefault(a => a.membershipType == "WORKSPACE").targetId;

            var sdt = DateTime.ParseExact(startTime, "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture);
            var edt = DateTime.ParseExact(endTime, "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture);

            var cnt = await _timesheetService.SyncTimeRecordsV2(workspaceId, login.token, sdt, edt);
            return new JsonResult(new
            {
                Message = $"Success Synced: {cnt}"
            });
        }

        [HttpPost]
        public async Task<IActionResult> UpdateSharepointUsers(string sharePointPeople)
        {
            //var employees = await _employeeService.ListAllAsync();
            //var employeeTitles = await _employeeTitleService.ListAllAsync();
            //// 更新每个员工的基本信息
            //foreach(var p in sharePointPeople)
            //{
            //    var employee = employees.FirstOrDefault(a => a.Email.ToLower() == p.EmailAddress.EmailAddress.ToLower());
            //    if(employee == null)
            //    {
            //        continue;
            //    }

            //    employee.DisplayName = p.DisplayName;
            //    employee.FamilyName = p.Surname;
            //    employee.GivenName = p.GivenName;
            //    employee.OfficeCity = p.WorkCity;
            //    employee.OfficeCountry = p.OfficeLocationsArray[0].Value;
            //    //employee.ProfilePicture = p

            //    var employeeTitle = employeeTitles.FirstOrDefault(b => b.EmoployeeGid == employee.Gid);
            //    if(employeeTitle == null)
            //    {
            //        employeeTitle = new EmployeeTitle()
            //        {

            //        }
            //    }
            //}

            var sharePointPeopled = Newtonsoft.Json.JsonConvert.DeserializeObject<List<SharePointPeopleViewModel>>(sharePointPeople);


            // 把SharePointUsers信息存到数据库里，再写两个Update语句就OK了
            await _sharePointUserService.Clear();
            var sharepointUsers = new List<SharePointUser>();
            foreach (var p in sharePointPeopled)
            {
                sharepointUsers.Add(new SharePointUser()
                {
                    ADObjectId = p.ADObjectId,
                    CompanyName = p.CompanyName,
                    CreationTimeString = p.CreationTimeString,
                    DisplayName = p.DisplayName,
                    DisplayNameFirstLast = p.DisplayNameFirstLast,
                    DisplayNameLastFirst = p.DisplayNameLastFirst,
                    EmailAddress = p.EmailAddress.EmailAddress,
                    GivenName = p.GivenName,
                    ImAddress = p.ImAddress,
                    OfficeCity = p.WorkCity,
                    OfficeCountry = p.OfficeLocationsArray == null ? "" : p.OfficeLocationsArray[0].Value,
                    PersonaTypeString = p.PersonaTypeString,
                    RelevanceScore = p.RelevanceScore,
                    Surname = p.Surname,
                    Title = p.Title,
                    WorkCity = p.WorkCity
                });
            }
            await _sharePointUserService.AddManyAsync(sharepointUsers);

            // 更新每个员工的基本信息
            var sql = "update Employee set FullName=S.DisplayName, FamilyName=S.Surname, GivenName=S.GivenName, OfficeCountry=S.OfficeCountry, OfficeCity=S.OfficeCity from SharePointUser S where S.EmailAddress=Email";
            var cnt = await _sharePointUserService.SqlExecuteNonQuery(sql);

            // 更新员工的Title信息
            sql = "update EmployeeTitle set RoleId=R.Id from SharePointUser S, Employee E, RoleTitle R where S.EmailAddress=E.Email and E.Gid=EmployeeTitle.EmployeeGid and S.Title=R.Title and R.type='internal'";
            cnt = await _sharePointUserService.SqlExecuteNonQuery(sql);

            return new JsonResult(new
            {
                Message = $"Success Synced: {sharepointUsers.Count}"
            });
        }


    }
}
