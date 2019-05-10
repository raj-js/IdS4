using RajsLibs.Abstraction.Key;
using System;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace RajsLibs.Abstraction.Repositories.Operations
{
    public interface IFirst<TEntity, in TKey>
        where TEntity : class, IKey<TKey>
        where TKey : IEquatable<TKey>
    {
        TEntity First(Expression<Func<TEntity, bool>> predicate);

        TEntity FirstOrDefault(Expression<Func<TEntity, bool>> predicate);
    }

    public interface IFirstAsync<TEntity, in TKey>
        where TEntity : class, IKey<TKey>
        where TKey : IEquatable<TKey>
    {
        Task<TEntity> FirstAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default(CancellationToken));

        Task<TEntity> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default(CancellationToken));
    }
}
