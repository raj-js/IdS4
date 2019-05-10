using RajsLibs.Abstraction.Key;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace RajsLibs.Abstraction.Repositories.Operations
{
    public interface IAdd<in TEntity, in TKey> 
        where TEntity : class, IKey<TKey>
        where TKey: IEquatable<TKey>
    {
        void Add(params TEntity[] entities);

        void Add(IEnumerable<TEntity> entities);
    }

    public interface IAddAsync<in TEntity, in TKey>
        where TEntity : class, IKey<TKey>
        where TKey : IEquatable<TKey>
    {
        Task AddAsync(TEntity entity, CancellationToken cancellationToken = default(CancellationToken));

        Task AddAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default(CancellationToken));
    }
}
