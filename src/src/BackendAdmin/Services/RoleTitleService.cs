using Microsoft.eShopWeb.ApplicationCore.Entities;
using Microsoft.eShopWeb.BackendAdmin.Interfaces;
using Microsoft.eShopWeb.Infrastructure.Data;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Microsoft.eShopWeb.BackendAdmin.Services
{
    public class RoleTitleService : BaseService<RoleTitle>, IRoleTitleService
    {
        public RoleTitleService(CatalogContext dbContext)
            : base(dbContext)
        {
        }

        public async Task<List<RoleTitle>> GetAllInternalRoles()
        {
            var roles = await WhereAsync(a => a.Type == "Internal" && a.IsDeleted == false);
            return roles.ToList();
        }

        public async Task<List<RoleTitle>> GetAllExternalRoles()
        {
            var roles = await WhereAsync(a => a.Type == "External" && a.IsDeleted == false);
            return roles.ToList();
        }

    }
}
