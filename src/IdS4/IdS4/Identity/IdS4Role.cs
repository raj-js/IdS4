using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace IdS4.Identity
{
    public class IdS4Role: IdentityRole
    {
        public virtual ICollection<IdS4RoleClaim> RoleClaims { get; set; }
        public virtual ICollection<IdS4UserRole> UserRoles { get; set; }
    }
}
