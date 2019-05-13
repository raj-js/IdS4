using RajsLibs.Key;
using RajsLibs.Repositories.Operations;
using System;

namespace RajsLibs.Repositories
{
    public interface ICmdRepository<TEntity, TKey> :
        IAdd<TEntity, TKey>,
        IUpdate<TEntity, TKey>,
        IDelete<TEntity, TKey>

        where TEntity : class
        where TKey : IEquatable<TKey>
    {

    }

    public interface ICmdAsyncRepository<TEntity, TKey> :
        IAddAsync<TEntity, TKey>,
        IUpdateAsync<TEntity, TKey>,
        IDeleteAsync<TEntity, TKey>

        where TEntity : class
        where TKey : IEquatable<TKey>
    {

    }
}
