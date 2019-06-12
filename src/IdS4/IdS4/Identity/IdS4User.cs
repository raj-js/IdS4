using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace IdS4.Identity
{
    public class IdS4User : IdentityUser
    {
        public virtual ICollection<IdS4UserClaim> UserClaims { get; set; }
        public virtual ICollection<IdS4UserRole> UserRoles { get; set; }
    }
}
