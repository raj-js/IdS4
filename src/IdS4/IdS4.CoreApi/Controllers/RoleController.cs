using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using AutoMapper;
using IdS4.CoreApi.Models.Paging;
using IdS4.CoreApi.Models.Results;
using IdS4.CoreApi.Models.Role;
using IdS4.CoreApi.Models.User;
using IdS4.DbContexts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace IdS4.CoreApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoleController : ControllerBase
    {
        private readonly ILogger<RoleController> _logger;
        private readonly IdS4IdentityDbContext _identityDb;
        private readonly IMapper _mapper;

        public RoleController(
            ILogger<RoleController> logger,
            IdS4IdentityDbContext identityDb,
            IMapper mapper)
        {
            _logger = logger;
            _identityDb = identityDb;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<Paged<VmRole>> Get([FromQuery]PageQuery query)
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

            var role = await _identityDb.Roles.FindAsync(id);
            if (role == null) return ApiResult.NotFound(id);

            var roleClaims = await _identityDb.RoleClaims.AsNoTracking().Where(s => s.RoleId.Equals(id)).ToListAsync();

            var vmRole = _mapper.Map<VmRole>(role);
            vmRole.RoleClaims = _mapper.Map<List<VmRoleClaim>>(roleClaims);
            return ApiResult.Success(vmRole);
        }
    }
}