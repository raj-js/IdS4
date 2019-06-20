using AutoMapper;
using IdS4.Application.Commands;
using IdS4.Application.Models.Role;
using IdS4.DbContexts;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using IdS4.Identity;
using Microsoft.EntityFrameworkCore;

namespace IdS4.Application.CommandHandlers
{
    public class EditRoleCommandHandler: IRequestHandler<EditRoleCommand, VmRole>
    {
        private readonly IdS4IdentityDbContext _identityDb;
        private readonly IMapper _mapper;

        public EditRoleCommandHandler(IdS4IdentityDbContext identityDb, IMapper mapper)
        {
            _identityDb = identityDb;
            _mapper = mapper;
        }

        public async Task<VmRole> Handle(EditRoleCommand request, CancellationToken cancellationToken)
        {
            var origin = await _identityDb.Roles.AsNoTracking().SingleOrDefaultAsync(s => s.Id == request.Role.Id, cancellationToken);
            if (origin == null) return null;
            origin.RoleClaims = _mapper.Map<List<IdS4RoleClaim>>(request.Role.RoleClaims);

            var vm = _mapper.Map(origin, request.Role);

            await MarkRoleClaimsDeleted(request.Role.Id, request.Role.RoleClaims, cancellationToken);

            var entity = _mapper.Map<IdS4Role>(vm);
            var entry = _identityDb.Attach(entity);
            entry.State = EntityState.Modified;
            await _identityDb.SaveChangesAsync(cancellationToken);

            return _mapper.Map<VmRole>(entity);
        }

        private async Task MarkRoleClaimsDeleted(string roleId, List<VmRoleClaim> changed, CancellationToken cancellationToken = default)
        {
            var origin = await _identityDb.RoleClaims
                .AsNoTracking()
                .Where(s => s.RoleId.Equals(roleId))
                .ToListAsync(cancellationToken);

            var deleted = origin.Where(s => changed.All(c => c.Id != s.Id));

            foreach (var item in deleted)
                _identityDb.Attach(item).State = EntityState.Deleted;
        }
    }
}
