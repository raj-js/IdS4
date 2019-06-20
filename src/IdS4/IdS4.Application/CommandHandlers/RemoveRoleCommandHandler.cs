using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using IdS4.Application.Commands;
using IdS4.DbContexts;
using MediatR;

namespace IdS4.Application.CommandHandlers
{
    public class RemoveRoleCommandHandler : IRequestHandler<RemoveRoleCommand, bool>
    {
        private readonly IdS4IdentityDbContext _identityDb;
        private readonly IMapper _mapper;

        public RemoveRoleCommandHandler(IdS4IdentityDbContext identityDb, IMapper mapper)
        {
            _identityDb = identityDb;
            _mapper = mapper;
        }

        public async Task<bool> Handle(RemoveRoleCommand request, CancellationToken cancellationToken)
        {
            foreach (var roleId in request.RoleIds.Split(','))
            {
                var role = await _identityDb.Roles.FindAsync(new object[] { int.Parse(roleId) }, cancellationToken: cancellationToken);
                if (role == null) continue;

                _identityDb.Roles.Remove(role);
            }
            return await _identityDb.SaveChangesAsync(cancellationToken) > 0;
        }
    }
}
