using RajsLibs.Abstraction.Key;
using RajsLibs.Abstraction.Repositories.Operations;
using System;

namespace RajsLibs.Abstraction.Repositories
{
    public interface IQueryRepository<TEntity, TKey> :
        IAll<TEntity, TKey>,
        ICount<TEntity, TKey>,
        ILongCount<TEntity, TKey>,
        IExists<TEntity, TKey>,
        IExistsByExpression<TEntity, TKey>,
        IFirst<TEntity, TKey>,
        ISingle<TEntity, TKey>,
        IMultiplue<TEntity, TKey>,
        IWhere<TEntity, TKey>,
        IPaging<TEntity, TKey>

        where TEntity : class, IKey<TKey>
        where TKey : IEquatable<TKey>
    {

    }

    public interface IQueryAsyncRepository<TEntity, TKey> :
        IAllAsync<TEntity, TKey>,
        ICountAsync<TEntity, TKey>,
        ILongCountAsync<TEntity, TKey>,
        IExistsAsync<TEntity, TKey>,
        IExistsByExpressionAsync<TEntity, TKey>,
        IFirstAsync<TEntity, TKey>,
        ISingleAsync<TEntity, TKey>,
        IMultiplueAsync<TEntity, TKey>,
        IWhereAsync<TEntity, TKey>,
        IPagingAsync<TEntity, TKey>

        where TEntity : class, IKey<TKey>
        where TKey : IEquatable<TKey>
    {

    }
}
