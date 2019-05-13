using IdentityServer4.EntityFramework.Entities;
using RajsLibs.Key;

namespace IdS4.Wrappers
{
    public class IdS4IdentityResource : IdentityResource, IKey<int>
    {
    }
}
