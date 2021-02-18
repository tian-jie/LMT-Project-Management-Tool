using Microsoft.eShopWeb.ApplicationCore.Interfaces;

namespace Microsoft.eShopWeb.ApplicationCore.Entities
{
    public class ProjectOwner : BaseEntity, IAggregateRoot
    {
        /// <summary>
        /// 主键ID
        /// </summary>
        public override int Id { get; set; }

        /// <summary>
        /// Clockify里的id字符串
        /// </summary>
        public virtual string ProjectGid { get; set; }

        /// <summary>
        /// Clockify里的id字符串
        /// </summary>
        public virtual string EmployeeGid { get; set; }

    }
}
