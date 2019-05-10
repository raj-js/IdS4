using RajsLibs.Abstraction.Key;
using System;

namespace RajsLibs.Abstraction.Repositories
{
    public interface IRepository<TEntity, TKey> :
        IQueryRepository<TEntity, TKey>,
        IQueryAsyncRepository<TEntity, TKey>,
        ICmdRepository<TEntity, TKey>,
        ICmdAsyncRepository<TEntity, TKey>

        where TEntity : class, IKey<TKey>
        where TKey : IEquatable<TKey>
    {

    }
}
