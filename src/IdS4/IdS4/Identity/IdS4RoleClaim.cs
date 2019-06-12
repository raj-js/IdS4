using Microsoft.AspNetCore.Identity;

namespace IdS4.Identity
{
    public class IdS4RoleClaim : IdentityRoleClaim<string>
    {
        public virtual IdS4Role Role { get; set; }
    }
}
