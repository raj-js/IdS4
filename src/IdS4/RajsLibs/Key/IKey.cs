using System;

namespace RajsLibs.Key
{
    public interface IKey<out T> where T : IEquatable<T>
    {
        T Id { get; }
    }
}
