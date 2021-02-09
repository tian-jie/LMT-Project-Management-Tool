using Microsoft.eShopWeb.ApplicationCore.Entities;
using Microsoft.eShopWeb.BackendAdmin.Interfaces;
using Microsoft.eShopWeb.Infrastructure.Data;

namespace Microsoft.eShopWeb.BackendAdmin.Services
{
    public class ProjectTaskService : BaseService<ProjectTask>, IProjectTaskService
    {
        public ProjectTaskService(CatalogContext timesheetContext)
            : base(timesheetContext)
        {

        }
    }
}
