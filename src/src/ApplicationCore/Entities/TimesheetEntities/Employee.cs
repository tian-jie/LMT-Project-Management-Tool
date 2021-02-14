using Ardalis.GuardClauses;
using Microsoft.eShopWeb.ApplicationCore.Interfaces;
using System;

namespace Microsoft.eShopWeb.ApplicationCore.Entities
{
    public class Employee : BaseEntity, IAggregateRoot
    {
        /// <summary>
        /// 主键ID
        /// </summary>
        public override int Id { get; set; }

        /// <summary>
        /// Clockify里的id字符串
        /// </summary>
        public virtual string Gid { get; set; }

        /// <summary>
        /// 邮箱
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// 员工姓名
        /// </summary>
        public string FullName { get; set; }

        /// <summary>
        /// 员工姓名
        /// </summary>
        public string DisplayName { get; set; }

        /// <summary>
        /// 员工姓名
        /// </summary>
        public string NickName { get; set; }

        /// <summary>
        /// 员工姓名
        /// </summary>
        public string FamilyName { get; set; }

        /// <summary>
        /// 员工姓名
        /// </summary>
        public string GivenName { get; set; }

        /// <summary>
        /// 哪个Office
        /// </summary>
        public string OfficeCountry { get; set; }

        /// <summary>
        /// 哪个Office
        /// </summary>
        public string OfficeCity { get; set; }

        /// <summary>
        /// 员工头像
        /// </summary>
        public string ProfilePicture { get; set; }

        /// <summary>
        /// 默认workspace，借用clockify里的概念
        /// </summary>
        public string DefaultWorkspace { get; set; }

        /// <summary>
        /// 员工状态
        /// </summary>
        public string Status { get; set; }

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
