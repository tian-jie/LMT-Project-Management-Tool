using Microsoft.eShopWeb.ApplicationCore.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Microsoft.eShopWeb.BusinessCore.Interfaces
{
    public interface IRoleTitleService : IBaseService<RoleTitle>
    {
        Task<List<RoleTitle>> GetAllInternalRoles();

        Task<List<RoleTitle>> GetAllExternalRoles();
    }
}
