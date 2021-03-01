using Microsoft.eShopWeb.ApplicationCore.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Microsoft.eShopWeb.BusinessCore.Interfaces
{
    public interface IAspNetMenuService : IBaseService<AspNetMenu>
    {
        Task<List<AspNetMenu>> GetCachedMenus();
    }
}
