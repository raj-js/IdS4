using RajsLibs.Repositories.Paging;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace RajsLibs.Repositories.Operations
{
    public interface IPaging<TEntity, in TKey>
        where TEntity : class
        where TKey : IEquatable<TKey>
    {
        IEnumerable<TEntity> Paging(IPageQuery<TEntity> query);
    }

    public interface IPagingAsync<TEntity, in TKey>
        where TEntity : class
        where TKey : IEquatable<TKey>
    {
        Task<IEnumerable<TEntity>> PagingAsync(IPageQuery<TEntity> query, CancellationToken cancellationToken = default(CancellationToken));
    }
}
