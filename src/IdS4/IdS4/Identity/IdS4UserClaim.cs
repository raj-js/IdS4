using System.Security.Claims;
using Microsoft.AspNetCore.Identity;

namespace IdS4.Identity
{
    public class IdS4UserClaim : IdentityUserClaim<string>
    {
        public virtual IdS4User User { get; set; }

        public IdS4UserClaim()
        {
            
        }

        public IdS4UserClaim(string userId, string type, string value)
        {
            base.UserId = userId;
            base.InitializeFromClaim(new Claim(type, value));
        }
    }
}
