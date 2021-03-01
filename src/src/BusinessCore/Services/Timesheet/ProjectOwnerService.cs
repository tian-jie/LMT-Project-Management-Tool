using Microsoft.eShopWeb.ApplicationCore.Entities;
using Microsoft.eShopWeb.BusinessCore.Interfaces;
using Microsoft.eShopWeb.Infrastructure.Data;

namespace Microsoft.eShopWeb.BusinessCore.Services
{
    public class ProjectOwnerService : BaseService<ProjectOwner>, IProjectOwnerService
    {
        public ProjectOwnerService(CatalogContext dbContext)
            : base(dbContext)
        {

        }
    }
}
