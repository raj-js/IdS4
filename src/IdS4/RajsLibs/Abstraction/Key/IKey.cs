using System;

namespace RajsLibs.Abstraction.Key
{
    public interface IKey<out T> where T : IEquatable<T>
    {
        T Id { get; }
    }
}
