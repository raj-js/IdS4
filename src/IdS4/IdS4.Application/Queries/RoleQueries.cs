using AutoMapper;
using IdS4.Application.Models.Paging;
using IdS4.DbContexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using IdS4.Application.Models.Role;
using Microsoft.EntityFrameworkCore;

namespace IdS4.Application.Queries
{
    public class RoleQueries : IRoleQueries
    {
        private readonly IdS4IdentityDbContext _identityDb;
        private readonly IMapper _mapper;

        public RoleQueries(
            IdS4IdentityDbContext identityDb,
            IMapper mapper)
        {
            _identityDb = identityDb;
            _mapper = mapper;
        }

        public async Task<Paged<VmRole>> GetRoles(PagingQuery query)
        {
            var roles = await _identityDb.Roles
                .AsNoTracking()
                .OrderBy(query.Sort ?? "Id")
                .Skip(query.Skip)
                .Take(query.Limit)
                .ToListAsync();

            return Paged<VmRole>.From(
                _mapper.Map<List<VmRole>>(roles),
                await _identityDb.Roles.AsNoTracking().CountAsync()
            );
        }

        public async Task<VmRole> GetRole(string id)
        {
            var role = await _identityDb.Roles
                .AsNoTracking()
                .SingleOrDefaultAsync(s => s.Id.Equals(id));
            if (role == null) return null;

            var roleClaims = await _identityDb.RoleClaims
                .AsNoTracking()
                .Where(s => s.RoleId.Equals(id))
                .ToListAsync();

            var vmRole = _mapper.Map<VmRole>(role);
            vmRole.RoleClaims = _mapper.Map<List<VmRoleClaim>>(roleClaims);

            return vmRole;
        }
    }
}
