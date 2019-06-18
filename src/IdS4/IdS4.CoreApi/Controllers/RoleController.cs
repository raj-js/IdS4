using AutoMapper;
using IdS4.CoreApi.Extensions;
using IdS4.CoreApi.Models.Results;
using IdS4.CoreApi.Models.Role;
using IdS4.DbContexts;
using IdS4.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using IdS4.Application.Models.Paging;
using IdS4.Application.Queries;

namespace IdS4.CoreApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [BearerAuthorize]
    public class RoleController : ControllerBase
    {
        private readonly ILogger<RoleController> _logger;
        private readonly IdS4IdentityDbContext _identityDb;
        private readonly IMapper _mapper;
        private readonly IRoleQueries _roleQueries;

        public RoleController(
            ILogger<RoleController> logger,
            IdS4IdentityDbContext identityDb,
            IMapper mapper, 
            IRoleQueries roleQueries)
        {
            _logger = logger;
            _identityDb = identityDb;
            _mapper = mapper;
            _roleQueries = roleQueries;
        }

        [HttpGet]
        public async Task<Paged<VmRole>> Get([FromQuery]PagingQuery query)
        {
            var roles = await _identityDb.Roles.AsNoTracking()
                .OrderBy(query.Sort ?? "Id")
                .Skip(query.Skip)
                .Take(query.Limit)
                .ToListAsync();

            return Paged<VmRole>.From(
                _mapper.Map<List<VmRole>>(roles),
                await _identityDb.Roles.AsNoTracking().CountAsync()
            );
        }

        [HttpGet("{id}")]
        public async Task<ApiResult> Get([FromRoute] string id)
        {
            if (string.IsNullOrEmpty(id)) return ApiResult.NotFound(id);

            var vmRole = await _roleQueries.GetRole(id);
            if (vmRole == null)
                return ApiResult.NotFound(id);

            return ApiResult.Success(vmRole);
        }

        [HttpPost]
        public async Task<ApiResult> Add([FromBody]VmRole vm)
        {
            if (!ModelState.IsValid) return ApiResult.Failure(ModelState);

            var role = _mapper.Map<IdS4Role>(vm);
            var entry = _identityDb.Attach(role);
            await _identityDb.SaveChangesAsync();

            return ApiResult.Success(_mapper.Map<VmRole>(entry.Entity));
        }

        [HttpPut]
        public async Task<ApiResult> Edit([FromBody] VmRole vm)
        {
            if (string.IsNullOrEmpty(vm.Id)) return ApiResult.NotFound(vm.Id);

            var origin = await _identityDb.Roles
                .AsNoTracking()
                .SingleOrDefaultAsync(s => s.Id.Equals(vm.Id));

            if (origin == null) return ApiResult.NotFound(vm.Id);

            origin.RoleClaims = _mapper.Map<List<IdS4RoleClaim>>(vm.RoleClaims);
            vm = _mapper.Map(origin, vm);
            await MarkRoleClaimsDeleted(vm.Id, vm.RoleClaims);

            var entity = _mapper.Map<IdS4Role>(vm);
            var entry = _identityDb.Attach(entity);
            entry.State = EntityState.Modified;
            await _identityDb.SaveChangesAsync();

            vm = _mapper.Map<VmRole>(entity);
            return ApiResult.Success(vm);
        }

        [HttpDelete("{ids}")]
        public async Task<ApiResult> Remove([FromRoute]string ids)
        {
            if (string.IsNullOrEmpty(ids)) return ApiResult.Failure();

            foreach (var id in ids.Split(","))
            {
                var entity = await _identityDb.Roles.FindAsync(id);
                if (entity == null) continue;

                _identityDb.Roles.Remove(entity);
            }
            await _identityDb.SaveChangesAsync();
            return ApiResult.Success();
        }

        #region privates

        private async Task MarkRoleClaimsDeleted(string roleId, List<VmRoleClaim> changed)
        {
            var origin = await _identityDb.RoleClaims
                .AsNoTracking()
                .Where(s => s.RoleId.Equals(roleId))
                .ToListAsync();

            var deleted = origin.Where(s => changed.All(c => c.Id != s.Id));

            foreach (var item in deleted)
                _identityDb.Attach(item).State = EntityState.Deleted;
        }

        #endregion
    }
}