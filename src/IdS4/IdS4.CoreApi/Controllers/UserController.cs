using AutoMapper;
using IdS4.CoreApi.Extensions;
using IdS4.CoreApi.Models.Paging;
using IdS4.CoreApi.Models.User;
using IdS4.DbContexts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using IdS4.CoreApi.Models.Results;
using IdS4.Identity;

namespace IdS4.CoreApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [BearerAuthorize]
    public class UserController : ControllerBase
    {
        private readonly ILogger<UserController> _logger;
        private readonly IdS4IdentityDbContext _identityDb;
        private readonly IMapper _mapper;

        public UserController(
            ILogger<UserController> logger, 
            IdS4IdentityDbContext identityDb, 
            IMapper mapper)
        {
            _logger = logger;
            _identityDb = identityDb;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<Paged<VmUser>> Get([FromQuery]PageQuery query)
        {
            var users = await _identityDb.Users.AsNoTracking()
                .OrderBy(query.Sort ?? "Id")
                .Skip(query.Skip)
                .Take(query.Limit)
                .ToListAsync();

            return Paged<VmUser>.From(
                _mapper.Map<List<VmUser>>(users),
                await _identityDb.Users.AsNoTracking().CountAsync()
            );
        }

        [HttpGet("{id}")]
        public async Task<ApiResult> Get([FromRoute] string id)
        {
            if (string.IsNullOrEmpty(id)) return ApiResult.NotFound(id);

            var user = await _identityDb.Users.FindAsync(id);
            if (user == null) return  ApiResult.NotFound(id);

            var userClaims = await _identityDb.UserClaims.AsNoTracking().Where(s => s.UserId.Equals(id)).ToListAsync();

            var vmUser = _mapper.Map<VmUser>(user);
            vmUser.UserClaims = _mapper.Map<List<VmUserClaim>>(userClaims);
            return ApiResult.Success(vmUser);
        }

        [HttpPut]
        public async Task<ApiResult> Edit([FromBody] VmUser vm)
        {
            if (string.IsNullOrEmpty(vm.Id)) return ApiResult.NotFound(vm.Id);

            var origin = await _identityDb.Users
                .AsNoTracking()
                .SingleOrDefaultAsync(s => s.Id.Equals(vm.Id));

            if (origin == null) return ApiResult.NotFound(vm.Id);

            origin.UserClaims = _mapper.Map<List<IdS4UserClaim>>(vm.UserClaims);

            vm = _mapper.Map(origin, vm);

            await MarkUserClaimsDeleted(vm.Id, vm.UserClaims);
            var entity = _mapper.Map<IdS4User>(vm);

            entity.NormalizedUserName = origin.NormalizedUserName;
            entity.NormalizedEmail = origin.NormalizedEmail;
            entity.PasswordHash = origin.PasswordHash;
            entity.SecurityStamp = origin.SecurityStamp;
            entity.PhoneNumberConfirmed = origin.PhoneNumberConfirmed;
            entity.PhoneNumber = origin.PhoneNumber;

            var entry = _identityDb.Attach(entity);
            entry.State = EntityState.Modified;
            await _identityDb.SaveChangesAsync();

            vm = _mapper.Map<VmUser>(entity);
            return ApiResult.Success(vm);
        }

        [HttpDelete("{ids}")]
        public async Task<ApiResult> Remove([FromRoute]string ids)
        {
            if (string.IsNullOrEmpty(ids)) return ApiResult.Failure();

            foreach (var id in ids.Split(","))
            {
                var entity = await _identityDb.Users.FindAsync(id);
                if (entity == null) continue;

                _identityDb.Users.Remove(entity);
            }
            await _identityDb.SaveChangesAsync();
            return ApiResult.Success();
        }

        #region privates

        private async Task MarkUserClaimsDeleted(string roleId, List<VmUserClaim> changed)
        {
            var origin = await _identityDb.UserClaims
                .AsNoTracking()
                .Where(s => s.UserId.Equals(roleId))
                .ToListAsync();

            var deleted = origin.Where(s => changed.All(c => c.Id != s.Id));

            foreach (var item in deleted)
                _identityDb.Attach(item).State = EntityState.Deleted;
        }

        #endregion
    }
}
