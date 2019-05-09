using System;

namespace IdS4.Abstraction.Repositories
{
    public interface IRepository<T, TKey> 
        where T: class, new()
        where TKey: IEquatable<T>
    {

    }
}
