using Ardalis.Specification;
using Microsoft.EntityFrameworkCore;
using Microsoft.eShopWeb.ApplicationCore.Entities;
using Microsoft.eShopWeb.ApplicationCore.Interfaces;
using Microsoft.eShopWeb.BackendAdmin.Interfaces;
using Microsoft.eShopWeb.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.eShopWeb.BackendAdmin.Services
{
    public class BaseService<T> : IBaseService<T>
        where T : BaseEntity, IAggregateRoot

    {

        private EfRepository<T> _efRepository;
        protected DbContext _dbContext;
        public Specification<T> Specification { get; set; }
        Specification<T> IBaseService<T>.Specification { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public BaseService(CatalogContext dbContext)
        {
            _dbContext = dbContext;
            _efRepository = new EfRepository<T>(dbContext);
        }

        public async Task<int> SqlExecuteNonQuery(string sql, CancellationToken cancellationToken = default)
        {
            return await _dbContext.Database.ExecuteSqlRawAsync(sql, cancellationToken);
        }

        public async Task<int> AddManyAsync(List<T> entities, CancellationToken cancellationToken = default)
        {
            return await _efRepository.AddManyAsync(entities, cancellationToken);
        }

        public async Task<T> AddAsync(T entity, CancellationToken cancellationToken = default)
        {
            return await _efRepository.AddAsync(entity, cancellationToken);
        }

        public async Task UpdateAsync(T entity, CancellationToken cancellationToken = default)
        {
            await _efRepository.UpdateAsync(entity, cancellationToken);
        }

        public async Task<IReadOnlyList<T>> ListAsync(ISpecification<T> spec, CancellationToken cancellationToken = default)
        {
            return await _efRepository.ListAsync(spec, cancellationToken);
        }

        public async Task<IReadOnlyList<T>> ListAllAsync(CancellationToken cancellationToken = default)
        {
            return await _efRepository.ListAllAsync(cancellationToken);
        }

        public async Task DeleteAsync(T entity, CancellationToken cancellationToken = default)
        {
            await _efRepository.DeleteAsync(entity, cancellationToken);
        }

        public async Task DeleteAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default)
        {
            await _efRepository.DeleteAsync(predicate, cancellationToken);
        }

        public virtual async Task<IQueryable<T>> WhereAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default)
        {
            return await _efRepository.WhereAsync(predicate, cancellationToken);
        }
    }
}
