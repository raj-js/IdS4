using AutoMapper;
using IdS4.Application.Commands;
using IdS4.DbContexts;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace IdS4.Application.CommandHandlers
{
    public class RemoveUserCommandHandler : IRequestHandler<RemoveUserCommand, bool>
    {
        private readonly IdS4IdentityDbContext _identityDb;
        private readonly IMapper _mapper;

        public RemoveUserCommandHandler(IdS4IdentityDbContext identityDb, IMapper mapper)
        {
            _identityDb = identityDb;
            _mapper = mapper;
        }

        public async Task<bool> Handle(RemoveUserCommand request, CancellationToken cancellationToken)
        {
            foreach (var userId in request.UserIds.Split(','))
            {
                var user = await _identityDb.Users.FindAsync(new object[] { int.Parse(userId) }, cancellationToken: cancellationToken);
                if (user == null) continue;

                _identityDb.Users.Remove(user);
            }
            return await _identityDb.SaveChangesAsync(cancellationToken) > 0;
        }
    }
}
