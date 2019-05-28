using IdentityServer4.Extensions;
using IdentityServer4.Models;
using IdentityServer4.Services;
using IdS4.DbContexts;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using IdS4.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace IdS4.Server.Services
{
    public class ProfileService: IProfileService
    {
        private readonly ILogger<ProfileService> _logger;
        private readonly IdS4IdentityDbContext _identityDb;
        private readonly SignInManager<IdS4User> _signInManager;

        public ProfileService(
            IdS4IdentityDbContext identityDb, 
            ILogger<ProfileService> logger,
            SignInManager<IdS4User> signInManager
            )
        {
            _identityDb = identityDb;
            _logger = logger;
            _signInManager = signInManager;
        }

        public virtual async Task GetProfileDataAsync(ProfileDataRequestContext context)
        {
            context.LogProfileRequest(_logger);

            if (context.RequestedClaimTypes.Any())
            {
                var user = await _identityDb.Users.FindAsync(context.Subject.GetSubjectId());
                if (user != null)
                {
                    var claims = await _identityDb.UserClaims
                        .Where(s => s.UserId.Equals(user.Id))
                        .ToListAsync();

                    context.AddRequestedClaims(claims.Select(s => new Claim(s.ClaimType, s.ClaimValue)));
                }
            }
            context.LogIssuedClaims(_logger);
        }

        public virtual async Task IsActiveAsync(IsActiveContext context)
        {
            _logger.LogDebug("IsActive called from: {caller}", context.Caller);

            var user = await _identityDb.Users.FindAsync(context.Subject.GetSubjectId());
            // context.IsActive = user?.

            if (user == null)
                context.IsActive = false;
            else
                context.IsActive = await _signInManager.CanSignInAsync(user);
        }
    }
}
