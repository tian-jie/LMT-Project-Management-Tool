using System;

namespace Microsoft.eShopWeb.BusinessCore.ViewModel
{
    public class TimeEntryWithRoleViewModel
    {
        /// <summary>
        /// 主键ID
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Clockify里的id字符串
        /// </summary>
        public string Gid { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// 用户id，用clockify里的userid字符串
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// 项目ID，clockify里的项目id字符串
        /// </summary>
        public string ProjectId { get; set; }

        /// <summary>
        /// TaskId，clockify里的项目id字符串
        /// </summary>
        public string TaskId { get; set; }

        /// <summary>
        /// 录入时间的那一天
        /// </summary>
        public DateTime Date { get; set; }

        /// <summary>
        /// 记录这个的时间，精确到小时，如1.5小时
        /// </summary>
        public decimal TotalHours { get; set; }

        /// <summary>
        /// 冗余的Task里的isBilliable信息
        /// </summary>
        public bool IsBillable { get; set; }

        public int RoleId { get; set; }

    }

}
