using Microsoft.eShopWeb.ApplicationCore.Entities;
using Microsoft.eShopWeb.BusinessCore.Interfaces;
using Microsoft.eShopWeb.Infrastructure.Data;

namespace Microsoft.eShopWeb.BusinessCore.Services
{
    public class ResourcePlanService : BaseService<ResourcePlan>, IResourcePlanService
    {
        public ResourcePlanService(CatalogContext dbContext)
            : base(dbContext)
        {
        }

    }
}
