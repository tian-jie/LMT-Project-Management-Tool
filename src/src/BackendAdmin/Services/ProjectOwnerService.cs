using Microsoft.eShopWeb.ApplicationCore.Entities;
using Microsoft.eShopWeb.BackendAdmin.Interfaces;
using Microsoft.eShopWeb.Infrastructure.Data;

namespace Microsoft.eShopWeb.BackendAdmin.Services
{
    public class ProjectOwnerService : BaseService<ProjectOwner>, IProjectOwnerService
    {
        public ProjectOwnerService(CatalogContext dbContext)
            : base(dbContext)
        {

        }
    }
}
