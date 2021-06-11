using Microsoft.eShopWeb.ApplicationCore.Entities;
using Microsoft.eShopWeb.BusinessCore.Interfaces;
using Microsoft.eShopWeb.Infrastructure.Data;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Microsoft.eShopWeb.BusinessCore.Services
{
    public class RoleTitleService : BaseService<RoleTitle>, IRoleTitleService
    {
        private List<RoleTitle> _internalRoles = null;
        private List<RoleTitle> _externalRoles = null;
        public RoleTitleService(CatalogContext dbContext)
            : base(dbContext)
        {
        }

        public async Task<List<RoleTitle>> GetAllInternalRoles()
        {
            if(_internalRoles!= null && _internalRoles.Count>0)
            {
                return _internalRoles;
            }
            _internalRoles = (await WhereAsync(a => a.Type == "Internal" && a.IsDeleted == false)).ToList();
            return _internalRoles;
        }

        public async Task<List<RoleTitle>> GetAllExternalRoles()
        {
            if (_externalRoles != null && _externalRoles.Count > 0)
            {
                return _externalRoles;
            }
            _externalRoles = (await WhereAsync(a => a.Type == "External" && a.IsDeleted == false)).ToList();
            return _externalRoles;
        }

    }
}
