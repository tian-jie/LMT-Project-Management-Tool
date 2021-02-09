using Ardalis.Specification;
using Microsoft.EntityFrameworkCore;
using Microsoft.eShopWeb.ApplicationCore.Entities;
using Microsoft.eShopWeb.ApplicationCore.Interfaces;
using Microsoft.eShopWeb.BackendAdmin.Interfaces;
using Microsoft.eShopWeb.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.eShopWeb.BackendAdmin.Services
{
    public class BaseService<T> : EfRepository<T>, IBaseService<T>
        where T : BaseEntity, IAggregateRoot

    {
        public Specification<T> Specification { get; set; }

        public BaseService(CatalogContext timesheetContext)
            : base(timesheetContext)
        {
        }

        public async Task<int> SqlExecuteNonQuery(string sql, CancellationToken cancellationToken = default)
        {
            return await _dbContext.Database.ExecuteSqlRawAsync(sql, cancellationToken);
        }

    }
}
