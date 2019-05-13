using IdentityServer4.EntityFramework.Entities;
using RajsLibs.Key;

namespace IdS4.Wrappers
{
    public class IdS4PersistedGrant : PersistedGrant, IKey<string>
    {
        public string Id => Key;
    }
}
