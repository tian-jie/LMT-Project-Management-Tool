using Microsoft.eShopWeb.ApplicationCore.Interfaces;
using System;

namespace Microsoft.eShopWeb.ApplicationCore.Entities
{
    public class EmployeeTitle : BaseEntity, IAggregateRoot
    {
        /// <summary>
        /// 主键ID
        /// </summary>
        public override int Id { get; set; }

        /// <summary>
        /// Clockify里的id字符串
        /// </summary>
        public virtual string EmployeeGid { get; set; }

        /// <summary>
        /// 月份
        /// </summary>
        public string Month { get; set; }

        /// <summary>
        /// RoleId
        /// </summary>
        public int RoleId { get; set; }

        /// <summary>
        /// isLatest
        /// </summary>
        public bool IsLatest { get; set; }

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
        public bool? IsDeleted { get; set; }
    }
}
