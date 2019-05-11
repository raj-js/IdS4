using RajsLibs.Key;
using RajsLibs.Repositories.Operations;
using System;

namespace RajsLibs.Repositories
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
        IPaging<TEntity, TKey>,
        IFind<TEntity, TKey>

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
        IPagingAsync<TEntity, TKey>,
        IFindAsync<TEntity, TKey>

        where TEntity : class, IKey<TKey>
        where TKey : IEquatable<TKey>
    {

    }
}
