using Microsoft.EntityFrameworkCore;
using RajsLibs.EfCore.Uow;
using RajsLibs.Key;
using RajsLibs.Repositories;
using RajsLibs.Repositories.Operations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace RajsLibs.Repository.EfCore
{
    public class RepositoryBase<TDbContext, TEntity, TKey> : IRepository<TEntity, TKey>
        where TDbContext : DbContext
        where TEntity : class, IKey<TKey>
        where TKey : IEquatable<TKey>
    {
        protected IEfUnitOfWork<TDbContext> UnitOfWork { get; }
        protected DbSet<TEntity> Set { get; }

        public RepositoryBase(IEfUnitOfWork<TDbContext> unitOfWork)
        {
            UnitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            Set = UnitOfWork.DbContext.Set<TEntity>();
        }

        #region Queries

        public IEnumerable<TEntity> All()
        {
            return Set.ToList();
        }

        public async Task<IEnumerable<TEntity>> AllAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            return await Set.ToListAsync(cancellationToken);
        }

        public int Count(Expression<Func<TEntity, bool>> predicate = null)
        {
            return Set.Count(predicate);
        }

        public async Task<int> CountAsync(Expression<Func<TEntity, bool>> predicate = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await Set.CountAsync(predicate, cancellationToken);

        }

        public bool Exists(TKey id)
        {
            return Set.Any(s => s.Id.Equals(id));
        }

        public bool Exists(Expression<Func<TEntity, bool>> predicate)
        {
            return Set.Any(predicate);
        }

        public async Task<bool> ExistsAsync(TKey id, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await Set.AnyAsync(s => s.Id.Equals(id), cancellationToken);
        }

        public async Task<bool> ExistsAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await Set.AnyAsync(predicate, cancellationToken);
        }

        public TEntity First(Expression<Func<TEntity, bool>> predicate)
        {
            return Set.First(predicate);
        }

        public async Task<TEntity> FirstAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await Set.FirstAsync(predicate, cancellationToken);
        }

        public TEntity FirstOrDefault(Expression<Func<TEntity, bool>> predicate)
        {
            return Set.FirstOrDefault(predicate);
        }

        public async Task<TEntity> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await Set.FirstOrDefaultAsync(predicate, cancellationToken);
        }

        public long LongCount(Expression<Func<TEntity, bool>> predicate = null)
        {
            return Set.LongCount(predicate);
        }

        public async Task<long> LongCountAsync(Expression<Func<TEntity, bool>> predicate = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await Set.LongCountAsync(predicate, cancellationToken);
        }

        public IEnumerable<TEntity> Multiple(params TKey[] ids)
        {
            return Set.Where(s => ids.Contains(s.Id));
        }

        public IEnumerable<TEntity> Multiple(IEnumerable<TKey> ids)
        {
            return Set.Where(s => ids.Contains(s.Id));
        }

        public async Task<IEnumerable<TEntity>> MultipleAsync(IEnumerable<TKey> ids, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await Set.Where(s => ids.Contains(s.Id)).ToListAsync();
        }

        public TEntity Single(Expression<Func<TEntity, bool>> predicate)
        {
            return Set.Single(predicate);
        }

        public async Task<TEntity> SingleAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await Set.SingleAsync(predicate, cancellationToken);
        }

        public TEntity SingleOrDefault(Expression<Func<TEntity, bool>> predicate)
        {
            return Set.SingleOrDefault(predicate);
        }

        public async Task<TEntity> SingleOrDefaultAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await Set.SingleOrDefaultAsync(predicate, cancellationToken);
        }

        public IEnumerable<TEntity> Where(Expression<Func<TEntity, bool>> predicate)
        {
            return Set.Where(predicate);
        }

        public async Task<IEnumerable<TEntity>> WhereAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await Set.Where(predicate).ToListAsync();
        }

        public IEnumerable<TEntity> Paging(IPageQuery<TEntity> query)
        {
            return Set.AsNoTracking()
                .Where(query.Predicate)
                .OrderBy(query.OrderBy)
                .Skip(query.Skip)
                .Take(query.Take)
                .ToList();
        }

        public async Task<IEnumerable<TEntity>> PagingAsync(IPageQuery<TEntity> query, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await Set.AsNoTracking()
                .Where(query.Predicate)
                .OrderBy(query.OrderBy)
                .Skip(query.Skip)
                .Take(query.Take)
                .ToListAsync();
        }

        #endregion

        #region Commands

        public void Add(params TEntity[] entities)
        {
            Set.AddRange(entities);
        }

        public void Add(IEnumerable<TEntity> entities)
        {
            Set.AddRange(entities);
        }

        public async Task AddAsync(TEntity entity, CancellationToken cancellationToken = default(CancellationToken))
        {
            await Set.AddAsync(entity, cancellationToken);
        }

        public async Task AddAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default(CancellationToken))
        {
            await Set.AddRangeAsync(entities, cancellationToken);
        }

        public void Delete(params TKey[] ids)
        {
            Set.RemoveRange(Multiple(ids));
        }

        public void Delete(IEnumerable<TKey> ids)
        {
            Set.RemoveRange(Multiple(ids));
        }

        public void Delete(params TEntity[] entities)
        {
            Set.RemoveRange(entities);
        }

        public void Delete(IEnumerable<TEntity> entities)
        {
            Set.RemoveRange(entities);
        }

        public async Task Delete(TKey id, CancellationToken cancellationToken = default(CancellationToken))
        {
            Set.Remove(await FindAsync(id));
        }

        public async Task Delete(IEnumerable<TKey> ids, CancellationToken cancellationToken = default(CancellationToken))
        {
            Set.RemoveRange(await MultipleAsync(ids));
        }

        public async Task Delete(TEntity entity, CancellationToken cancellationToken = default(CancellationToken))
        {
            Set.Remove(entity);
            await Task.CompletedTask;
        }

        public async Task Delete(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default(CancellationToken))
        {
            Set.RemoveRange(entities);
            await Task.CompletedTask;
        }

        public void Update(params TEntity[] entities)
        {
            Set.UpdateRange(entities);
        }

        public void Update(IEnumerable<TEntity> entities)
        {
            Set.UpdateRange(entities);
        }

        public async Task UpdateAsync(TEntity entity, CancellationToken cancellationToken = default(CancellationToken))
        {
            Set.Update(entity);
            await Task.CompletedTask;
        }

        public async Task UpdateAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default(CancellationToken))
        {
            Set.UpdateRange(entities);
            await Task.CompletedTask;
        }

        public TEntity Find(TKey id)
        {
            return Set.Find(id);
        }

        public async Task<TEntity> FindAsync(TKey id)
        {
            return await Set.FindAsync(id);
        }

        #endregion
    }
}
