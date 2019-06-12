using Microsoft.AspNetCore.Identity;

namespace IdS4.Identity
{
    public class IdS4UserRole : IdentityUserRole<string>
    {
        public virtual IdS4User User { get; set; }
        public virtual IdS4Role Role { get; set; }
    }
}
