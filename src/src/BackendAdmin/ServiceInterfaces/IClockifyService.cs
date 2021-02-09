using Kevin.T.Clockify.Data.Models;
using Microsoft.eShopWeb.ApplicationCore.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Microsoft.eShopWeb.BackendAdmin.ServiceInterfaces
{
    public interface IClockifyService
    {
        public Task<LoginModel> Login(string username, string password);


        public Task<List<UserGroupModel>> GetUserGroups(string userid, string token);

        public Task<List<UserModel>> GetUsers(string userid, string token);

        public Task<List<TimeEntryModel>> GetTimeEntries(string userid, string token, DateTime startDate, DateTime endDate);



        public Task<List<TimeEntryModelV2>> GetTimeEntriesV2(string userid, string token, DateTime startDate, DateTime endDate);

        public Task<List<TimeEntry>> GetTimeEntriesV3(string userid, string token, DateTime startDate, DateTime endDate);


        public Task<TimeEntryResponseModelV2> GetTimeEntriesByPageV2(string userid, string token, int pageId, DateTime startDate, DateTime endDate);


        public Task<List<ClientModel>> GetClients(string userid, string token);

        public Task<List<ProjectModel>> GetProjects(string userid, string token);

    }
}
