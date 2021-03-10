using Microsoft.eShopWeb.ApplicationCore.Entities;
using Microsoft.eShopWeb.BusinessCore.Interfaces;
using Microsoft.eShopWeb.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Microsoft.eShopWeb.BusinessCore.Services
{
    public class AspNetUserService : BaseService<AspNetUser>, IAspNetUserService
    {
        public AspNetUserService(CatalogContext dbContext)
            : base(dbContext)
        {

        }

    }
}
