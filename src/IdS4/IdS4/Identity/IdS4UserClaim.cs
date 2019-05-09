using IdS4.Abstraction.Events;
using Microsoft.AspNetCore.Identity;

namespace IdS4.Identity
{
    public class IdS4UserClaim : IdentityUserClaim<string>, IHasEventsManager
    {
        public IEventsManager EventsManager => throw new System.NotImplementedException();
    }
}
