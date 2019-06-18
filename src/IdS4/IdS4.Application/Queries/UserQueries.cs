using AutoMapper;
using IdS4.Application.Models.Paging;
using IdS4.CoreApi.Models.User;
using IdS4.DbContexts;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;

namespace IdS4.Application.Queries
{
    public class UserQueries : IUserQueries
    {
        private readonly IdS4IdentityDbContext _identityDb;
        private readonly IMapper _mapper;

        public UserQueries(
            IdS4IdentityDbContext identityDb,
            IMapper mapper)
        {
            _identityDb = identityDb;
            _mapper = mapper;
        }

        public async Task<Paged<VmUser>> GetUsers(PagingQuery query)
        {
            var users = await _identityDb.Users
                .AsNoTracking()
                .OrderBy(query.Sort ?? "Id")
                .Skip(query.Skip)
                .Take(query.Limit)
                .ToListAsync();

            return Paged<VmUser>.From(
                _mapper.Map<List<VmUser>>(users),
                await _identityDb.Users.AsNoTracking().CountAsync()
            );
        }

        public async Task<VmUser> GetUser(string id)
        {
            var user = await _identityDb.Users.AsNoTracking().SingleOrDefaultAsync(s => s.Id.Equals(id));
            if (user == null) return null;

            var userClaims = await _identityDb.UserClaims
                .AsNoTracking()
                .Where(s => s.UserId.Equals(id))
                .ToListAsync();

            var vmUser = _mapper.Map<VmUser>(user);
            vmUser.UserClaims = _mapper.Map<List<VmUserClaim>>(userClaims);

            return vmUser;
        }
    }
}
