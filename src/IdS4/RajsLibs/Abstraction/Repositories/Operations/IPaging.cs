using RajsLibs.Abstraction.Key;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace RajsLibs.Abstraction.Repositories.Operations
{
    public interface IPageQuery<TEntity>
        where TEntity : class
    {
        Expression<Func<TEntity, bool>> Predicate { get; }

        int Skip { get; }

        int Take { get; }

        string Order { get; }

        bool IsDesc { get; }

        /// <summary>
        /// Order + IsDesc
        /// </summary>
        string OrderBy { get; }
    }

    public interface IPaging<TEntity, in TKey>
        where TEntity : class, IKey<TKey>
        where TKey : IEquatable<TKey>
    {
        IEnumerable<TEntity> Paging(IPageQuery<TEntity> query);
    }

    public interface IPagingAsync<TEntity, in TKey>
        where TEntity : class, IKey<TKey>
        where TKey : IEquatable<TKey>
    {
        Task<IEnumerable<TEntity>> PagingAsync(IPageQuery<TEntity> query, CancellationToken cancellationToken = default(CancellationToken));
    }
}
