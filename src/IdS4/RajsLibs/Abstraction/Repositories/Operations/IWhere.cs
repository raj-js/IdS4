using RajsLibs.Abstraction.Key;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace RajsLibs.Abstraction.Repositories.Operations
{
    public interface IWhere<TEntity, in TKey>
        where TEntity : class, IKey<TKey>
        where TKey : IEquatable<TKey>
    {
        IEnumerable<TEntity> Where(Expression<Func<TEntity, bool>> predicate);
    }

    public interface IWhereAsync<TEntity, in TKey>
        where TEntity : class, IKey<TKey>
        where TKey : IEquatable<TKey>
    {
        Task<IEnumerable<TEntity>> WhereAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default(CancellationToken));
    }
}
