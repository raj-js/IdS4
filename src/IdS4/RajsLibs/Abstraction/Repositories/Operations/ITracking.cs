using RajsLibs.Abstraction.Key;
using System;
using System.Collections.Generic;

namespace RajsLibs.Abstraction.Repositories.Operations
{
    public interface ITracking<in TEntity, in TKey>
        where TEntity : class, IKey<TKey>
        where TKey : IEquatable<TKey>
    {
        void Tracking(params TEntity[] entities);

        void Tracking(IEnumerable<TEntity> entities);

        void NoTracking(params TEntity[] entities);

        void NoTracking(IEnumerable<TEntity> entities);
    }
}
