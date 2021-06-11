using Microsoft.eShopWeb.ApplicationCore.Interfaces;

namespace Microsoft.eShopWeb.ApplicationCore.Entities
{
    public class OrgChart : BaseEntity, IAggregateRoot
    {
        /// <summary>
        /// 主键ID
        /// </summary>
        public override int Id { get; set; }

        public string FullName { get; set; }
        public string Email { get; set; }
        public string UserName { get; set; }
        public string JobTitle { get; set; }
        public string ProfileImageAddress { get; set; }
        public string ItemSource { get; set; }
        public string AadObjectId { get; set; }
        public string SupervisorId { get; set; }
    }
}
