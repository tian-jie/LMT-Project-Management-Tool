using Microsoft.eShopWeb.ApplicationCore.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Microsoft.eShopWeb.BusinessCore.Interfaces
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
        /// <param name="timeEntries">这个时间段所有的timeEntries，为了提升性能，如果没有的话，里面自己取一遍</param>
        /// <returns>by Role进行汇总，这个时间段，每天，这个Task的各个角色的AC统计</returns>
        Task<List<EffortUsedByRoleByDate>> CalculateACByRoleAndSave(string projectGid, string taskGid, DateTime startDate, DateTime endDate, List<TimeEntry> timeEntries = null);


        /// <summary>
        /// 根据这个时间段取得相关timeentry的task信息
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        Task<List<ProjectTask>> GetAvaliableTaskByTimeEntry(DateTime startDate, DateTime endDate);

    }
}
