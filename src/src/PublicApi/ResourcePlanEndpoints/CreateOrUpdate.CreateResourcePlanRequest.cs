using System;

namespace Microsoft.eShopWeb.PublicApi.CreateResourcePlanEndpoints
{
    public class CreateResourcePlanRequest : BaseRequest
    {
        /// <summary>
        /// 主键ID
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Clockify里的id字符串
        /// </summary>
        public virtual string ProjectGid { get; set; }

        /// <summary>
        /// EmployeeGid
        /// </summary>
        public string EmployeeGid { get; set; }

        /// <summary>
        /// TaskName
        /// </summary>
        public string TaskName { get; set; }

        /// <summary>
        /// Year
        /// </summary>
        public int Year { get; set; }

        /// <summary>
        /// Week
        /// </summary>
        public int Week { get; set; }

        /// <summary>
        /// Amount
        /// </summary>
        public decimal Amount { get; set; }

        /// <summary>
        /// 百分比，乘100的数
        /// </summary>
        public int Workload { get; set; }

        /// <summary>
        /// Duration
        /// </summary>
        public string start_date { get; set; }

        /// <summary>
        /// Duration
        /// </summary>
        public string end_date { get; set; }


        /// <summary>
        /// 创建日期，框架直接调用
        /// </summary>
        public DateTime? CreatedDate { get; set; }

        /// <summary>
        /// 创建人ID，框架直接调用
        /// </summary>
        public string CreatedUserID { get; set; }

        /// <summary>
        /// 创建人，框架直接调用
        /// </summary>
        public string CreatedUserName { get; set; }

        /// <summary>
        /// 更新日期，框架直接调用
        /// </summary>
        public DateTime? UpdatedDate { get; set; }

        /// <summary>
        /// 更新人ID，框架直接调用
        /// </summary>
        public string UpdatedUserID { get; set; }

        /// <summary>
        /// 更新人，框架直接调用
        /// </summary>
        public string UpdatedUserName { get; set; }

        /// <summary>
        /// 逻辑删除，框架直接调用
        /// </summary>
        public bool IsDeleted { get; set; }
    }

}
