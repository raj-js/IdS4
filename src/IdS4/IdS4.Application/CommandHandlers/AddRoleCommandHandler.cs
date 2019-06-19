using IdS4.Application.Commands;
using IdS4.Application.Models.Role;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using IdS4.DbContexts;
using IdS4.Identity;

namespace IdS4.Application.CommandHandlers
{
    public class AddRoleCommandHandler :
        IRequestHandler<AddRoleCommand, VmRole>
    {
        private readonly IdS4IdentityDbContext _identityDb;
        private readonly IMapper _mapper;

        public AddRoleCommandHandler(IdS4IdentityDbContext identityDb, IMapper mapper)
        {
            _identityDb = identityDb;
            _mapper = mapper;
        }


        public async Task<VmRole> Handle(AddRoleCommand request, CancellationToken cancellationToken)
        {
            var role = _mapper.Map<IdS4Role>(request.Role);
            var entry = await _identityDb.AddAsync(role, cancellationToken);
            await _identityDb.SaveChangesAsync(cancellationToken);

            return _mapper.Map<VmRole>(entry.Entity);
        }
    }
}
