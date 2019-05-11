using RajsLibs.Key;
using System;

namespace RajsLibs.Repositories
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
