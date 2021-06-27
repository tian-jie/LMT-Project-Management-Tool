using Microsoft.eShopWeb.ApplicationCore.Entities;
using Microsoft.eShopWeb.BusinessCore.ViewModel;
using System.Collections.Generic;

namespace Microsoft.eShopWeb.Web.Services
{
    public class ProjectStatisticsViewModel
    {
        public List<EffortUsedViewModel> EffortUsedByDay { get; set; }
        public List<EffortUsedViewModel> EffortUsedByRole { get; set; }
        public List<EstimateEffort> EstimateEffort { get; set; }
    }
}
