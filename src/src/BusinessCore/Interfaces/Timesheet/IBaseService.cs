using Ardalis.Specification;
using Microsoft.eShopWeb.ApplicationCore.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.eShopWeb.BusinessCore.Interfaces
{
    public interface IBaseService<T>
        where T: BaseEntity
    {
        Specification<T> Specification { get; set; }

        Task<int> AddManyAsync(List<T> entities, CancellationToken cancellationToken = default);

        Task<T> AddAsync(T entity, CancellationToken cancellationToken = default);

        Task UpdateAsync(T entity, CancellationToken cancellationToken = default);

        Task<int> SqlExecuteNonQuery(string sql, CancellationToken cancellationToken = default);

        Task<IReadOnlyList<T>> ListAsync(ISpecification<T> spec, CancellationToken cancellationToken = default);

        Task<IReadOnlyList<T>> ListAllAsync(CancellationToken cancellationToken = default);

        Task DeleteAsync(T entity, CancellationToken cancellationToken = default);

        Task DeleteAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default);

        Task<IQueryable<T>> WhereAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default);

    }
}
