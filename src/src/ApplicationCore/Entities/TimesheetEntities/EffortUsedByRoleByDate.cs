using Microsoft.eShopWeb.ApplicationCore.Interfaces;
using System;

namespace Microsoft.eShopWeb.ApplicationCore.Entities
{
    public class EffortUsedByRoleByDate : BaseEntity, IAggregateRoot
    {
        /// <summary>
        /// 主键ID
        /// </summary>
        public override int Id { get; set; }

        public string ProjectGid { get; set; }

        public string TaskGid { get; set; }

        public DateTime Date { get; set; }
        
        public int RoleId { get; set; }

        public string RoleName { get; set; }

        public string RoleShortTitle { get; set; }

        public string RoleCategory { get; set; }

        public decimal RoleRate { get; set; }

        public decimal TotalHours { get; set; }

        public decimal TotalHoursRate { get; set; }

    }
}
