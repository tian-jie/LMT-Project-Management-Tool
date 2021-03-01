using Microsoft.eShopWeb.ApplicationCore.Entities;
using Microsoft.eShopWeb.BusinessCore.Interfaces;
using Microsoft.eShopWeb.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Microsoft.eShopWeb.BusinessCore.Services
{
    public class AspNetMenuService : BaseService<AspNetMenu>, IAspNetMenuService
    {
        public AspNetMenuService(CatalogContext dbContext)
            : base(dbContext)
        {

        }

        public async Task<List<AspNetMenu>> GetCachedMenus()
        {
            var cache = MemoryCacheHelper.GetInstance();
            return await cache.GetOrCreateAsync("AspNetMenu", async entry =>
            {
                entry.SlidingExpiration = TimeSpan.MaxValue;
                var menus = await ListAllAsync();
                return menus.ToList();
            });
        }

    }
}
