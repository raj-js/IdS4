using IdS4.Abstraction.Events;
using Microsoft.AspNetCore.Identity;

namespace IdS4.Identity
{
    public class IdS4User : IdentityUser, IHasEventsManager
    {
        public IEventsManager EventsManager { get; private set; }
    }
}
