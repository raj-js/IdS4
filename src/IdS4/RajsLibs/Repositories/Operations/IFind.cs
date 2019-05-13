using RajsLibs.Key;
using System;
using System.Threading.Tasks;

namespace RajsLibs.Repositories.Operations
{
    public interface IFind<TEntity, in TKey>
        where TEntity : class
        where TKey : IEquatable<TKey>
    {
        TEntity Find(TKey id);
    }

    public interface IFindAsync<TEntity, in TKey>
        where TEntity : class
        where TKey : IEquatable<TKey>
    {
        Task<TEntity> FindAsync(TKey id);
    }
}
