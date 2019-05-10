using RajsLibs.Abstraction.Key;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace RajsLibs.Abstraction.Repositories.Operations
{
    public interface IUpdate<in TEntity, in TKey>
        where TEntity : class, IKey<TKey>
        where TKey : IEquatable<TKey>
    {
        void Update(params TEntity[] entities);

        void Update(IEnumerable<TEntity> entities);
    }

    public interface IUpdateAsync<in TEntity, in TKey>
        where TEntity : class, IKey<TKey>
        where TKey : IEquatable<TKey>
    {
        Task UpdateAsync(TEntity entity, CancellationToken cancellationToken = default(CancellationToken));

        Task UpdateAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default(CancellationToken));
    }
}
