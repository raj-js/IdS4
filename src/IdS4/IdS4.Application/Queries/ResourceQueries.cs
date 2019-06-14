using AutoMapper;
using IdS4.Application.Models.Paging;
using IdS4.Application.Models.Resource;
using IdS4.DbContexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;


namespace IdS4.Application.Queries
{
    public class ResourceQueries : IResourceQueries
    {
        private readonly IdS4ConfigurationDbContext _configurationDb;
        private readonly ILogger<ResourceQueries> _logger;
        private readonly IMapper _mapper;

        public ResourceQueries(IdS4ConfigurationDbContext configurationDb, ILogger<ResourceQueries> logger, IMapper mapper)
        {
            _configurationDb = configurationDb;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<Paged<VmIdentityResource>> GetIdentityResources(PagingQuery query)
        {
            return Paged<VmIdentityResource>.From(
                _mapper.Map<List<VmIdentityResource>>(
                    await _configurationDb.IdentityResources
                        .AsNoTracking()
                        .OrderBy(query.Sort ?? "Id")
                        .Skip(query.Skip)
                        .Take(query.Limit)
                        .ToListAsync()
                    ),
                await _configurationDb.IdentityResources
                    .AsNoTracking()
                    .CountAsync()
            );
        }

        public async Task<VmIdentityResource> GetIdentityResource(int id)
        {
            var entity = await _configurationDb.IdentityResources
                .AsNoTracking()
                .SingleOrDefaultAsync(s => s.Id == id);

            entity.UserClaims = await _configurationDb.IdentityClaims
                .AsNoTracking()
                .Where(s => s.IdentityResourceId == entity.Id)
                .ToListAsync();

            entity.Properties = await _configurationDb.IdentityResourceProperties
                .AsNoTracking()
                .Where(s => s.IdentityResourceId == entity.Id)
                .ToListAsync();

            return _mapper.Map<VmIdentityResource>(entity);

        }

        public async Task<Paged<VmApiResource>> GetApiResources(PagingQuery query)
        {
            return Paged<VmApiResource>.From(
                _mapper.Map<List<VmApiResource>>(
                    await _configurationDb.ApiResources
                        .AsNoTracking()
                        .OrderBy(query.Sort ?? "Id")
                        .Skip(query.Skip)
                        .Take(query.Limit)
                        .ToListAsync()
                    ),
                await _configurationDb.ApiResources
                    .AsNoTracking()
                    .CountAsync()
            );
        }

        public async Task<VmApiResource> GetApiResource(int id)
        {
            var entity = await _configurationDb.ApiResources
                .AsNoTracking()
                .SingleOrDefaultAsync(s=>s.Id == id);

            entity.UserClaims = await _configurationDb.ApiResourceClaims
                .AsNoTracking()
                .Where(s => s.ApiResourceId == entity.Id)
                .ToListAsync();

            entity.Properties = await _configurationDb.ApiResourceProperties
                .AsNoTracking()
                .Where(s => s.ApiResourceId == entity.Id)
                .ToListAsync();

            entity.Scopes = await _configurationDb.ApiScopes
                .AsNoTracking()
                .Where(s => s.ApiResourceId == entity.Id)
                .ToListAsync();

            entity.Secrets = await _configurationDb.ApiSecrets
                .AsNoTracking()
                .Where(s => s.ApiResourceId == entity.Id)
                .ToListAsync();

            foreach (var scope in entity.Scopes)
            {
                scope.UserClaims =
                    await _configurationDb.ApiScopeClaims
                        .AsNoTracking()
                        .Where(s => s.ApiScopeId == scope.Id)
                        .ToListAsync();
            }

            return _mapper.Map<VmApiResource>(entity);
        }
    }
}
