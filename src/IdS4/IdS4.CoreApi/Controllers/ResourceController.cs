using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using IdentityServer4.EntityFramework.Entities;
using IdS4.Application.Commands;
using IdS4.Application.Models.Paging;
using IdS4.Application.Models.Resource;
using IdS4.Application.Queries;
using IdS4.CoreApi.Extensions;
using IdS4.CoreApi.Models.Results;
using IdS4.DbContexts;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

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
        private readonly IResourceQueries _resourceQueries;
        private readonly IMediator _mediator;

        public ResourceController(IdS4ConfigurationDbContext configurationDb, ILogger<ResourceController> logger, IMapper mapper, IResourceQueries resourceQueries, IMediator mediator)
        {
            _configurationDb = configurationDb;
            _logger = logger;
            _mapper = mapper;
            _resourceQueries = resourceQueries;
            _mediator = mediator;
        }

        [HttpGet("identity")]
        public async Task<Paged<VmIdentityResource>> GetIdentityResources([FromQuery]PagingQuery query)
        {
            return await _resourceQueries.GetIdentityResources(query);
        }

        [HttpGet("api")]
        public async Task<Paged<VmApiResource>> GetApiResources([FromQuery] PagingQuery query)
        {
            return await _resourceQueries.GetApiResources(query);
        }

        [HttpGet("api/{id}")]
        public async Task<ApiResult> GetApiResource([FromRoute] int id)
        {
            if (id <= 0) return ApiResult.NotFound(id);

            return ApiResult.Success(await _resourceQueries.GetApiResource(id));
        }

        [HttpGet("identity/{id}")]
        public async Task<ApiResult> GetIdentityResource([FromRoute] int id)
        {
            if (id <= 0) return ApiResult.NotFound(id);

            return ApiResult.Success(await _resourceQueries.GetIdentityResource(id));
        }

        [HttpPost("identity")]
        public async Task<ApiResult> AddIdentityResource([FromBody]VmIdentityResource vm)
        {
            if (vm.Id > 0) return ApiResult.NotFound(vm.Id);

            var command = new AddIdentityResourceCommand(vm);
            return ApiResult.Success(await _mediator.Send(command));
        }

        [HttpPut("identity")]
        public async Task<ApiResult> EditIdentityResource([FromBody]VmIdentityResource vm)
        {
            if (vm.Id <= 0 ||
                !await _configurationDb.IdentityResources.AsNoTracking().AnyAsync(s => s.Id == vm.Id))
                return ApiResult.NotFound(vm.Id);

            var resource = _mapper.Map<IdentityResource>(vm);
            var entry = _configurationDb.Attach(resource);
            entry.State = EntityState.Modified;
            await _configurationDb.SaveChangesAsync();

            var vmResource = _mapper.Map<VmIdentityResource>(entry.Entity);
            return ApiResult.Success(vmResource);
        }

        [HttpPut("identity/claims/{resourceId}")]
        public async Task<ApiResult> EditIdentityClaims([FromRoute]int resourceId, [FromBody]List<VmIdentityClaim> claims)
        {
            if (resourceId <= 0 ||
                !await _configurationDb.IdentityResources.AsNoTracking().AnyAsync(s => s.Id == resourceId))
                return ApiResult.NotFound(resourceId);

            var claimEntities = await _configurationDb.IdentityClaims
                .AsNoTracking()
                .Where(s => s.IdentityResourceId == resourceId)
                .ToListAsync();

            var entities = new List<IdentityClaim>();

            foreach (var claim in claims)
            {
                var entity = _mapper.Map<IdentityClaim>(claim);

                if (entity.Id == default)
                    _configurationDb.Add((object) entity);
                else
                {
                    var entry = _configurationDb.Attach(entity);
                    entry.State = EntityState.Modified;
                }
                entities.Add(entity);
            }

            claimEntities.Where(s => entities.All(e => e.Id != s.Id))
                .ToList()
                .ForEach(s =>
                {
                    var entry = _configurationDb.Attach(s);
                    entry.State = EntityState.Deleted;
                });

            await _configurationDb.SaveChangesAsync();

            return ApiResult.Success(entities);
        }

        [HttpPut("identity/properties/{resourceId}")]
        public async Task<ApiResult> EditIdentityProperties([FromRoute] int resourceId,
            [FromBody] List<VmIdentityResourceProperty> properties)
        {
            if (resourceId <= 0 ||
                !await _configurationDb.IdentityResources.AsNoTracking().AnyAsync(s => s.Id == resourceId))
                return ApiResult.NotFound(resourceId);

            var propertyEntities = await _configurationDb.IdentityResourceProperties
                .AsNoTracking()
                .Where(s => s.IdentityResourceId == resourceId)
                .ToListAsync();

            var entities = new List<IdentityResourceProperty>();

            foreach (var property in properties)
            {
                var entity = _mapper.Map<IdentityResourceProperty>(property);

                if (entity.Id == default)
                    _configurationDb.Add((object) entity);
                else
                {
                    var entry = _configurationDb.Attach(entity);
                    entry.State = EntityState.Modified;
                }
                entities.Add(entity);
            }

            propertyEntities.Where(s => entities.All(e => e.Id != s.Id))
                .ToList()
                .ForEach(s =>
                {
                    var entry = _configurationDb.Attach(s);
                    entry.State = EntityState.Deleted;
                });

            await _configurationDb.SaveChangesAsync();

            return ApiResult.Success(entities);
        }

        [HttpDelete("identity/{resourceIds}")]
        public async Task<ApiResult> RemoveIdentityResource([FromRoute] string resourceIds)
        {
            if (string.IsNullOrEmpty(resourceIds)) return ApiResult.Failure();

            foreach (var resourceId in resourceIds.Split(","))
            {
                var resource = await _configurationDb.IdentityResources.FindAsync(int.Parse(resourceId));
                if (resource == null) continue;

                _configurationDb.IdentityResources.Remove(resource);
            }
            await _configurationDb.SaveChangesAsync();
            return ApiResult.Success();
        }

        [HttpPost("api")]
        public async Task<ApiResult> AddApiResource([FromBody]VmApiResource vm)
        {
            if (vm.Id > 0) return ApiResult.NotFound(vm.Id);

            var resource = _mapper.Map<ApiResource>(vm);

            var entry = await _configurationDb.ApiResources.AddAsync(resource);
            await _configurationDb.SaveChangesAsync();

            return ApiResult.Success(entry.Entity);
        }

        [HttpPut("api")]
        public async Task<ApiResult> EditApiResource([FromBody]VmApiResource vm)
        {
            if (vm.Id <= 0 ||
                !await _configurationDb.ApiResources.AsNoTracking().AnyAsync(s => s.Id == vm.Id))
                return ApiResult.NotFound(vm.Id);

            var resource = _mapper.Map<VmApiResource>(vm);
            var entry = _configurationDb.Attach(resource);
            entry.State = EntityState.Modified;
            await _configurationDb.SaveChangesAsync();

            return ApiResult.Success(entry.Entity);
        }

        [HttpDelete("api/{resourceIds}")]
        public async Task<ApiResult> RemoveApiResource([FromRoute]string resourceIds)
        {
            foreach (var resourceId in resourceIds.Split(","))
            {
                var resource = await _configurationDb.ApiResources.FindAsync(int.Parse(resourceId));
                if (resource == null) continue;

                _configurationDb.ApiResources.Remove(resource);
            }
            await _configurationDb.SaveChangesAsync();
            return ApiResult.Success();
        }

        [HttpPut("api/scopes/{resourceId}")]
        public async Task<ApiResult> EditApiScopes([FromRoute] int resourceId, [FromBody] List<VmApiScope> scopes)
        {
            if (resourceId <= 0 ||
                !await _configurationDb.ApiResources.AsNoTracking().AnyAsync(s => s.Id == resourceId))
                return ApiResult.NotFound(resourceId);

            var scopeEntities = await _configurationDb.ApiScopes
                .AsNoTracking()
                .Where(s => s.ApiResourceId == resourceId)
                .ToListAsync();

            var entities = new List<ApiScope>();
            foreach (var scope in scopes)
            {
                var claimEntities = await _configurationDb.ApiScopeClaims
                    .AsNoTracking()
                    .Where(s => s.ApiScopeId == scope.Id)
                    .ToListAsync();

                claimEntities.Where(ce => scope.UserClaims.All(c => c.Id != ce.Id))
                    .ToList()
                    .ForEach(ce =>
                    {
                        var entry = _configurationDb.Attach(ce);
                        entry.State = EntityState.Deleted;
                    });

                var entity = _mapper.Map<ApiScope>(scope);
                if (entity.Id == default)
                    _configurationDb.Add((object) entity);
                else
                {
                    var entry = _configurationDb.Attach(entity);
                    entry.State = EntityState.Modified;
                }
                entities.Add(entity);
            }

            scopeEntities.Where(s => entities.All(e => e.Id != s.Id))
                .ToList()
                .ForEach(s =>
                {
                    var entry = _configurationDb.Attach(s);
                    entry.State = EntityState.Deleted;
                });

            await _configurationDb.SaveChangesAsync();

            var vm = _mapper.Map<List<VmApiScope>>(entities);
            return ApiResult.Success(vm);
        }

        [HttpPut("api/secrets/{resourceId}")]
        public async Task<ApiResult> EditApiSecrets([FromRoute] int resourceId, [FromBody] List<VmApiSecret> secrets)
        {
            if (resourceId <= 0 ||
                !await _configurationDb.ApiResources.AsNoTracking().AnyAsync(s => s.Id == resourceId))
                return ApiResult.NotFound(resourceId);

            var secretEntities = await _configurationDb.ApiSecrets
                .AsNoTracking()
                .Where(s => s.ApiResourceId == resourceId)
                .ToListAsync();

            var entities = new List<ApiSecret>();

            foreach (var secret in secrets)
            {
                var entity = _mapper.Map<ApiSecret>(secret);

                if (entity.Id == default)
                    _configurationDb.Add((object) entity);
                else
                {
                    var entry = _configurationDb.Attach(entity);
                    entry.State = EntityState.Modified;
                }
                entities.Add(entity);
            }

            secretEntities.Where(s => entities.All(e => e.Id != s.Id))
                .ToList()
                .ForEach(s =>
                {
                    var entry = _configurationDb.Attach(s);
                    entry.State = EntityState.Deleted;
                });

            await _configurationDb.SaveChangesAsync();

            return ApiResult.Success(entities);
        }

        [HttpPut("api/claims/{resourceId}")]
        public async Task<ApiResult> EditApiResourceClaims([FromRoute] int resourceId, [FromBody] List<VmApiResourceClaim> claims)
        {
            if (resourceId <= 0 ||
                !await _configurationDb.ApiResources.AsNoTracking().AnyAsync(s => s.Id == resourceId))
                return ApiResult.NotFound(resourceId);

            var claimEntities = await _configurationDb.ApiResourceClaims
                .AsNoTracking()
                .Where(s => s.ApiResourceId == resourceId)
                .ToListAsync();

            var entities = new List<ApiResourceClaim>();

            foreach (var claim in claims)
            {
                var entity = _mapper.Map<ApiResourceClaim>(claim);

                if (entity.Id == default)
                    _configurationDb.Add((object) entity);
                else
                {
                    var entry = _configurationDb.Attach(entity);
                    entry.State = EntityState.Modified;
                }
                entities.Add(entity);
            }

            claimEntities.Where(s => entities.All(e => e.Id != s.Id))
                .ToList()
                .ForEach(s =>
                {
                    var entry = _configurationDb.Attach(s);
                    entry.State = EntityState.Deleted;
                });

            await _configurationDb.SaveChangesAsync();

            return ApiResult.Success(entities);
        }

        [HttpPut("api/properties/{resourceId}")]
        public async Task<ApiResult> EditApiResourceProperties([FromRoute] int resourceId, [FromBody] List<VmApiResourceProperty> properties)
        {
            if (resourceId <= 0 ||
                !await _configurationDb.ApiResources.AsNoTracking().AnyAsync(s => s.Id == resourceId))
                return ApiResult.NotFound(resourceId);

            var propertyEntities = await _configurationDb.ApiResourceProperties
                .AsNoTracking()
                .Where(s => s.ApiResourceId == resourceId)
                .ToListAsync();

            var entities = new List<ApiResourceProperty>();

            foreach (var property in properties)
            {
                var entity = _mapper.Map<ApiResourceProperty>(property);

                if (entity.Id == default)
                    _configurationDb.Add((object) entity);
                else
                {
                    var entry = _configurationDb.Attach(entity);
                    entry.State = EntityState.Modified;
                }
                entities.Add(entity);
            }

            propertyEntities.Where(s => entities.All(e => e.Id != s.Id))
                .ToList()
                .ForEach(s =>
                {
                    var entry = _configurationDb.Attach(s);
                    entry.State = EntityState.Deleted;
                });

            await _configurationDb.SaveChangesAsync();

            return ApiResult.Success(entities);
        }
    }
}