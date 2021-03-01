using System;
using System.Threading.Tasks;

namespace Microsoft.eShopWeb.BusinessCore.Interfaces
{
    public interface ITimesheetService
    {
        Task<int> SyncUsers(string userid, string token);

        Task<int> SyncGroups(string userid, string token);

        Task<int> SyncTimeRecordsV2(string userid, string token, DateTime startDate, DateTime endDate);
        Task<int> SyncTimeRecordsV3(string userid, string token, DateTime startTime, DateTime endTime);

        Task<int> SyncProjects(string userid, string token);

        Task<int> SyncClients(string userid, string token);

    }
}
