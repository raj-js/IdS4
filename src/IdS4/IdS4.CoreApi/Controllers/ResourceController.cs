using System.Collections.Generic;
using IdentityServer4.EntityFramework.Entities;
using IdS4.CoreApi.Extensions;
using IdS4.CoreApi.Models.Paging;
using IdS4.DbContexts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using AutoMapper;
using IdS4.CoreApi.Models.Resource;
using IdS4.CoreApi.Models.Results;

namespace IdS4.CoreApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [BearerAuthorize]
    public class ResourceController : ControllerBase
    {
        private readonly IdS4ConfigurationDbContext _configurationDb;
        private readonly ILogger<ResourceController> _logger;
        private readonly IMapper _mapper;

        public ResourceController(IdS4ConfigurationDbContext configurationDb, ILogger<ResourceController> logger, IMapper mapper)
        {
            _configurationDb = configurationDb;
            _logger = logger;
            _mapper = mapper;
        }

        [HttpGet("identity")]
        public async Task<Paged<IdentityResource>> GetIdentityResources([FromQuery]PageQuery query)
        {
            return Paged<IdentityResource>.From(
                await _configurationDb.IdentityResources.AsNoTracking()
                    .OrderBy(query.Sort ?? "Id")
                    .Skip(query.Skip)
                    .Take(query.Limit)
                    .ToListAsync(),
                await _configurationDb.IdentityResources.AsNoTracking().CountAsync()
                );
        }

        [HttpPost("identity/add")]
        public async Task<ApiResult> AddIdentityResource([FromBody]VmIdentityResource vm)
        {
            //if (!ModelState.IsValid)
            //    return ModelState.ToApiResult();

            if (vm.Id > 0) return ApiResult.NotFound(vm.Id);

            var resource = _mapper.Map<IdentityResource>(vm);

            var entry = await _configurationDb.IdentityResources.AddAsync(resource);
            await _configurationDb.SaveChangesAsync();

            return ApiResult.Success(entry.Entity);
        }

        [HttpGet("identity/{id}")]
        public async Task<ApiResult> GetIdentityResource([FromRoute] int id)
        {
            if (id <= 0) return ApiResult.NotFound(id);

            var entity = await _configurationDb.IdentityResources.FindAsync(id);
            entity.UserClaims = await _configurationDb.IdentityClaims.Where(s => s.IdentityResourceId == entity.Id).ToListAsync();
            entity.Properties = await _configurationDb.IdentityResourceProperties.Where(s => s.IdentityResourceId == entity.Id).ToListAsync();

            var resource = _mapper.Map<VmIdentityResource>(entity);
            return ApiResult.Success(resource);
        }

        [HttpPost("identity/edit")]
        public async Task<ApiResult> EditIdentityResource([FromBody]VmIdentityResource vm)
        {
            if (vm.Id <= 0 ||
                !await _configurationDb.IdentityResources.AsNoTracking().AnyAsync(s => s.Id == vm.Id))
                return ApiResult.NotFound(vm.Id);

            var resource = _mapper.Map<IdentityResource>(vm);
            var entry = _configurationDb.Attach(resource);
            entry.State = EntityState.Modified;
            await _configurationDb.SaveChangesAsync();

            return ApiResult.Success(entry.Entity);
        }

        [HttpPost("identity/editClaims/{resourceId}")]
        public async Task<ApiResult> EditIdentityClaims([FromRoute]int resourceId, [FromBody]List<VmIdentityClaim> claims)
        {
            if (resourceId <= 0 ||
                !await _configurationDb.IdentityResources.AsNoTracking().AnyAsync(s => s.Id == resourceId))
                return ApiResult.NotFound(resourceId);

            var entities = new List<IdentityClaim>();

            // don't use foreach
            for (var i = 0; i < claims.Count; i++)
            {
                var entity = _mapper.Map<IdentityClaim>(claims[i]);

                if (entity.Id == default)
                    _configurationDb.Add(entity);
                else
                {
                    var entry = _configurationDb.Attach(entity);
                    entry.State = EntityState.Modified;
                }
                entities.Add(entity);
            }
            await _configurationDb.SaveChangesAsync();

            return ApiResult.Success(entities);
        }
    }
}