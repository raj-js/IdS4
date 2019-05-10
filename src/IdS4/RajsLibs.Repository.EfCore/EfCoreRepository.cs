using Microsoft.EntityFrameworkCore;
using RajsLibs.Abstraction.Key;
using RajsLibs.Abstraction.Repositories;
using RajsLibs.Abstraction.Repositories.Operations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace RajsLibs.Repository.EfCore
{
    public class EfCoreRepository<TEntity, TKey> : IRepository<TEntity, TKey>
        where TEntity : class, IKey<TKey>
        where TKey : IEquatable<TKey>
    {
        private readonly DbContext _dbContext;

        public EfCoreRepository(DbContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        #region Queries

        public IEnumerable<TEntity> All()
        {
            return _dbContext.Set<TEntity>();
        }

        public async Task<IEnumerable<TEntity>> AllAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            return await _dbContext.Set<TEntity>().ToListAsync(cancellationToken);
        }

        public int Count(Expression<Func<TEntity, bool>> predicate = null)
        {
            return _dbContext.Set<TEntity>().Count(predicate);
        }

        public async Task<int> CountAsync(Expression<Func<TEntity, bool>> predicate = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await _dbContext.Set<TEntity>().CountAsync(predicate, cancellationToken);

        }

        public bool Exists(TKey id)
        {
            return _dbContext.Set<TEntity>().Any(s => s.Id.Equals(id));
        }

        public bool Exists(Expression<Func<TEntity, bool>> predicate)
        {
            return _dbContext.Set<TEntity>().Any(predicate);
        }

        public async Task<bool> ExistsAsync(TKey id, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await _dbContext.Set<TEntity>().AnyAsync(s => s.Id.Equals(id), cancellationToken);
        }

        public async Task<bool> ExistsAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await _dbContext.Set<TEntity>().AnyAsync(predicate, cancellationToken);
        }

        public TEntity First(Expression<Func<TEntity, bool>> predicate)
        {
            return _dbContext.Set<TEntity>().First(predicate);
        }

        public async Task<TEntity> FirstAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await _dbContext.Set<TEntity>().FirstAsync(predicate, cancellationToken);
        }

        public TEntity FirstOrDefault(Expression<Func<TEntity, bool>> predicate)
        {
            return _dbContext.Set<TEntity>().FirstOrDefault(predicate);
        }

        public async Task<TEntity> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await _dbContext.Set<TEntity>().FirstOrDefaultAsync(predicate, cancellationToken);
        }

        public long LongCount(Expression<Func<TEntity, bool>> predicate = null)
        {
            return _dbContext.Set<TEntity>().LongCount(predicate);
        }

        public async Task<long> LongCountAsync(Expression<Func<TEntity, bool>> predicate = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await _dbContext.Set<TEntity>().LongCountAsync(predicate, cancellationToken);
        }

        public IEnumerable<TEntity> Multiple(params TKey[] ids)
        {
            return _dbContext.Set<TEntity>().Where(s => ids.Contains(s.Id));
        }

        public IEnumerable<TEntity> Multiple(IEnumerable<TKey> ids)
        {
            return _dbContext.Set<TEntity>().Where(s => ids.Contains(s.Id));
        }

        public async Task<IEnumerable<TEntity>> MultipleAsync(IEnumerable<TKey> ids, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await _dbContext.Set<TEntity>().Where(s => ids.Contains(s.Id)).ToListAsync();
        }

        public TEntity Single(Expression<Func<TEntity, bool>> predicate)
        {
            return _dbContext.Set<TEntity>().Single(predicate);
        }

        public async Task<TEntity> SingleAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await _dbContext.Set<TEntity>().SingleAsync(predicate, cancellationToken);
        }

        public TEntity SingleOrDefault(Expression<Func<TEntity, bool>> predicate)
        {
            return _dbContext.Set<TEntity>().SingleOrDefault(predicate);
        }

        public async Task<TEntity> SingleOrDefaultAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await _dbContext.Set<TEntity>().SingleOrDefaultAsync(predicate, cancellationToken);
        }

        public IEnumerable<TEntity> Where(Expression<Func<TEntity, bool>> predicate)
        {
            return _dbContext.Set<TEntity>().Where(predicate);
        }

        public async Task<IEnumerable<TEntity>> WhereAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await _dbContext.Set<TEntity>().Where(predicate).ToListAsync();
        }

        public IEnumerable<TEntity> Paging(IPageQuery<TEntity> query)
        {
            return _dbContext.Set<TEntity>()
                .Where(query.Predicate)
                //.OrderBy(query.Order)
                .Skip(query.Skip)
                .Take(query.Take)
                .ToList();
        }

        public async Task<IEnumerable<TEntity>> PagingAsync(IPageQuery<TEntity> query, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await _dbContext.Set<TEntity>()
                .Where(query.Predicate)
                //.OrderBy(query.Order)
                .Skip(query.Skip)
                .Take(query.Take)
                .ToListAsync();
        }

        #endregion

        #region Commands

        public void Add(params TEntity[] entities)
        {
            _dbContext.AddRange(entities);
        }

        public void Add(IEnumerable<TEntity> entities)
        {
            _dbContext.AddRange(entities);
        }

        public async Task AddAsync(TEntity entity, CancellationToken cancellationToken = default(CancellationToken))
        {
            await _dbContext.AddAsync(entity, cancellationToken);
        }

        public async Task AddAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default(CancellationToken))
        {
            await _dbContext.AddRangeAsync(entities, cancellationToken);
        }

        public void Delete(params TKey[] ids)
        {

        }

        public void Delete(IEnumerable<TKey> ids)
        {
            throw new NotImplementedException();
        }

        public void Delete(params TEntity[] entities)
        {
            throw new NotImplementedException();
        }

        public void Delete(IEnumerable<TEntity> entities)
        {
            throw new NotImplementedException();
        }

        public Task Delete(TKey id, CancellationToken cancellationToken = default(CancellationToken))
        {
            throw new NotImplementedException();
        }

        public Task Delete(IEnumerable<TKey> ids, CancellationToken cancellationToken = default(CancellationToken))
        {
            throw new NotImplementedException();
        }

        public Task Delete(TEntity entity, CancellationToken cancellationToken = default(CancellationToken))
        {
            throw new NotImplementedException();
        }

        public Task Delete(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default(CancellationToken))
        {
            throw new NotImplementedException();
        }

        public void Update(params TEntity[] entities)
        {
            throw new NotImplementedException();
        }

        public void Update(IEnumerable<TEntity> entities)
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(TEntity entity, CancellationToken cancellationToken = default(CancellationToken))
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default(CancellationToken))
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
