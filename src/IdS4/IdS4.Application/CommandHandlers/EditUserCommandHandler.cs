using AutoMapper;
using IdS4.Application.Commands;
using IdS4.Application.Models.User;
using IdS4.DbContexts;
using IdS4.Identity;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace IdS4.Application.CommandHandlers
{
    public class EditUserCommandHandler : IRequestHandler<EditUserCommand, VmUser>
    {
        private readonly IdS4IdentityDbContext _identityDb;
        private readonly IMapper _mapper;

        public EditUserCommandHandler(IdS4IdentityDbContext identityDb, IMapper mapper)
        {
            _identityDb = identityDb;
            _mapper = mapper;
        }

        public async Task<VmUser> Handle(EditUserCommand request, CancellationToken cancellationToken)
        {
            var origin = await _identityDb.Users
                .AsNoTracking()
                .SingleOrDefaultAsync(s => s.Id.Equals(request.User.Id), cancellationToken);

            if (origin == null) return null;
            origin.UserClaims = _mapper.Map<List<IdS4UserClaim>>(request.User.UserClaims);

            var vm = _mapper.Map(origin, request.User);

            await MarkUserClaimsDeleted(request.User.Id, request.User.UserClaims, cancellationToken);
            var entity = _mapper.Map<IdS4User>(vm);

            entity.NormalizedUserName = origin.NormalizedUserName;
            entity.NormalizedEmail = origin.NormalizedEmail;
            entity.PasswordHash = origin.PasswordHash;
            entity.SecurityStamp = origin.SecurityStamp;
            entity.PhoneNumberConfirmed = origin.PhoneNumberConfirmed;
            entity.PhoneNumber = origin.PhoneNumber;

            var entry = _identityDb.Attach(entity);
            entry.State = EntityState.Modified;
            await _identityDb.SaveChangesAsync(cancellationToken);

            return _mapper.Map<VmUser>(entity);
        }

        private async Task MarkUserClaimsDeleted(string roleId, List<VmUserClaim> changed, CancellationToken cancellationToken = default)
        {
            var origin = await _identityDb.UserClaims
                .AsNoTracking()
                .Where(s => s.UserId.Equals(roleId))
                .ToListAsync(cancellationToken);

            var deleted = origin.Where(s => changed.All(c => c.Id != s.Id));

            foreach (var item in deleted)
                _identityDb.Attach(item).State = EntityState.Deleted;
        }
    }
}
