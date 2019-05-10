using RajsLibs.Abstraction.Key;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace RajsLibs.Abstraction.Repositories.Operations
{
    public interface IDelete<in TEntity, in TKey>
        where TEntity : class, IKey<TKey>
        where TKey : IEquatable<TKey>
    {
        void Delete(params TKey[] ids);

        void Delete(IEnumerable<TKey> ids);

        void Delete(params TEntity[] entities);

        void Delete(IEnumerable<TEntity> entities);
    }

    public interface IDeleteAsync<in TEntity, in TKey>
        where TEntity : class, IKey<TKey>
        where TKey : IEquatable<TKey>
    {
        Task Delete(TKey id, CancellationToken cancellationToken = default(CancellationToken));

        Task Delete(IEnumerable<TKey> ids, CancellationToken cancellationToken = default(CancellationToken));

        Task Delete(TEntity entity, CancellationToken cancellationToken = default(CancellationToken));

        Task Delete(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default(CancellationToken));
    }
}
