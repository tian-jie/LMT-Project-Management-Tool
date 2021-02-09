using System;

namespace Microsoft.eShopWeb.BackendAdmin.ViewModels
{
    public class EffortUsedAccountingViewModel
    {
        public int RoleId { get; set; }

        public string RoleName { get; set; }

        public decimal RoleRate { get; set; }

        public decimal TotalHours { get; set; }

        public decimal TotalHoursRate { get; set; }

    }

}
