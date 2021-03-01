using Microsoft.eShopWeb.ApplicationCore.Entities;
using Microsoft.eShopWeb.BusinessCore.Interfaces;
using Microsoft.eShopWeb.Infrastructure.Data;

namespace Microsoft.eShopWeb.BusinessCore.Services
{
    public class ProjectTaskService : BaseService<ProjectTask>, IProjectTaskService
    {
        public ProjectTaskService(CatalogContext dbContext)
            : base(dbContext)
        {

        }
    }
}
