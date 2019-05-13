using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace RajsLibs.Repositories.Operations
{
    public interface IAll<out TEntity, in TKey>
        where TEntity : class
        where TKey : IEquatable<TKey>
    {
        IEnumerable<TEntity> All();
    }

    public interface IAllAsync<TEntity, in TKey>
        where TEntity : class
        where TKey : IEquatable<TKey>
    {
        Task<IEnumerable<TEntity>> AllAsync(CancellationToken cancellationToken = default(CancellationToken));
    }
}
