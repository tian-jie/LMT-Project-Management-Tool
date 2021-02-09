using Microsoft.eShopWeb.ApplicationCore.Entities;
using System;
using System.Threading.Tasks;

namespace Microsoft.eShopWeb.BackendAdmin.Interfaces
{
    public interface ITimeEntryService : IBaseService<TimeEntry>
    {


        /// <summary>
        /// 计算AC，根据某个日期段，某个Task
        /// </summary>
        /// <param name="projectGid">projectGid</param>
        /// <param name="taskGid">taskGid，可以为空，如果一个project下有task，则空task作为一个task计算</param>
        /// <param name="startDate">时间段</param>
        /// <param name="endDate">时间段</param>
        /// <returns>by Role进行汇总，这个时间段，每天，这个Task的各个角色的AC统计</returns>
        Task CalculateACByRoleAndSave(string projectGid, string taskGid, DateTime startDate, DateTime endDate);

    }
}
