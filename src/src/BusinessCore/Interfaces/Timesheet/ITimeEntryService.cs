using Microsoft.eShopWeb.ApplicationCore.Entities;
using Microsoft.eShopWeb.BusinessCore.ViewModel;
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
        Task<List<EffortUsedByRoleByDate>> CalculateACByRole(string projectGid, string taskGid, DateTime startDate, DateTime endDate, List<TimeEntry> timeEntries = null);


        /// <summary>
        /// 根据这个时间段取得相关timeentry的task信息
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        Task<List<ProjectTask>> GetAvaliableTaskByTimeEntry(DateTime startDate, DateTime endDate);

        /// <summary>
        /// 根据给定的GroupId和指定的时间段，获取TimeEntry
        /// </summary>
        /// <param name="groupId">groupId，如果为空，则不过滤这个字段，全部查询</param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        Task<List<TimeEntry>> GetTimeEntriesByGroupAndDurationAsync(string groupId, DateTime startDate, DateTime endDate);

        /// <summary>
        /// 统计项目花费信息，by到每一个成员
        /// </summary>
        /// <param name="projectGid"></param>
        /// <returns></returns>
        Task<List<TimeEntriesGroupByEmployeeView>> GetTimeEntriesByProjectGroupByEmployee(string projectGid);

        /// <summary>
        /// 统计项目花费信息
        /// </summary>
        /// <param name="projectGid"></param>
        /// <returns></returns>
        Task<ProjectAccountingView> GetTimeEntriesByProject(string projectGid);

        /// <summary>
        /// 获取员工在这段时间内的每个项目的花费时间
        /// </summary>
        /// <param name="employeeId"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        Task<List<TimesheetByWeekView>> GetTimeEntriesByEmployeeGroupByProject(string employeeId, DateTime startDate, DateTime endDate);

        /// <summary>
        /// 获取某个项目填写过的员工
        /// </summary>
        /// <param name="projectGid"></param>
        /// <returns></returns>
        Task<List<EmployeeView>> GetEmployeeByProjectTimeEntry(string projectGid);

        /// <summary>
        /// 获取某个项目填写过的员工，到指定周，不包含当周的数据
        /// </summary>
        /// <param name="projectGid"></param>
        /// <param name="date"></param>
        /// <returns></returns>
        Task<List<EffortByWeekView>> GetActualEffortByWeek(string projectGid, DateTime date);

    }
}
