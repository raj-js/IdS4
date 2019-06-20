using System.Collections.Generic;
using System.Security.Claims;
using AutoMapper;
using IdS4.Application.Commands;
using IdS4.Application.Models.User;
using IdS4.DbContexts;
using IdS4.Identity;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System.Threading;
using System.Threading.Tasks;
using IdentityModel;

namespace IdS4.Application.CommandHandlers
{
    public class AddUserCommandHandler : IRequestHandler<AddUserCommand, VmUserAdd>
    {
        private readonly IdS4IdentityDbContext _identityDb;
        private readonly IMapper _mapper;
        private readonly UserManager<IdS4User> _userManager;

        public AddUserCommandHandler(IdS4IdentityDbContext identityDb, IMapper mapper, UserManager<IdS4User> userManager)
        {
            _identityDb = identityDb;
            _mapper = mapper;
            _userManager = userManager;
        }

        public async Task<VmUserAdd> Handle(AddUserCommand request, CancellationToken cancellationToken)
        {
            var user = _mapper.Map<IdS4User>(request.User);

            // 默认密码可以放置到配置文件或者系统设置中
            const string defaultPassword = "Ad123!@#";

            var identityResult = await _userManager.CreateAsync(user, defaultPassword);
            if (!identityResult.Succeeded) return VmUserAdd.Failure(identityResult);

            await _identityDb.UserClaims.AddRangeAsync(new List<IdS4UserClaim>
                {
                    new IdS4UserClaim(user.Id, JwtClaimTypes.Id, user.Id),
                    new IdS4UserClaim(user.Id, JwtClaimTypes.Subject, user.Id),
                    new IdS4UserClaim(user.Id, JwtClaimTypes.Name, user.UserName),
                    new IdS4UserClaim(user.Id, JwtClaimTypes.Email, user.Email),
                    new IdS4UserClaim(user.Id, JwtClaimTypes.EmailVerified, user.EmailConfirmed.ToString())
                }, 
                cancellationToken
            );
            await _identityDb.SaveChangesAsync(cancellationToken);
            return VmUserAdd.Success(_mapper.Map<VmUser>(user), defaultPassword);
        }
    }
}
