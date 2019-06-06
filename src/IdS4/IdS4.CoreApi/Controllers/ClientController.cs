using AutoMapper;
using IdS4.CoreApi.Models.Client;
using IdS4.CoreApi.Models.Paging;
using IdS4.CoreApi.Models.Results;
using IdS4.DbContexts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;

namespace IdS4.CoreApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClientController : ControllerBase
    {
        private readonly IdS4ConfigurationDbContext _configurationDb;
        private readonly ILogger<ResourceController> _logger;
        private readonly IMapper _mapper;

        public ClientController(IdS4ConfigurationDbContext configurationDb, ILogger<ResourceController> logger, IMapper mapper)
        {
            _configurationDb = configurationDb;
            _logger = logger;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<Paged<VmClient>> GetClients([FromQuery]PageQuery query)
        {
            var clients = await _configurationDb.Clients.AsNoTracking()
                .OrderBy(query.Sort ?? "Id")
                .Skip(query.Skip)
                .Take(query.Limit)
                .ToListAsync();

            return Paged<VmClient>.From(
                _mapper.Map<List<VmClient>>(clients),
                await _configurationDb.Clients.AsNoTracking().CountAsync()
            );
        }

        [HttpGet("/{id}")]
        public async Task<ApiResult> GetClient([FromRoute]int id)
        {
            if (id <= 0) return ApiResult.NotFound(id);

            var client = await _configurationDb.Clients.FindAsync(id);
            if (client == null) return ApiResult.NotFound(id);

            client.ClientSecrets =
                await _configurationDb.ClientSecrets.Where(s => s.ClientId == client.Id).ToListAsync();
            client.AllowedGrantTypes =
                await _configurationDb.ClientGrantTypes.Where(s => s.ClientId == client.Id).ToListAsync();
            client.RedirectUris =
                await _configurationDb.ClientRedirectUris.Where(s => s.ClientId == client.Id).ToListAsync();
            client.PostLogoutRedirectUris =
                await _configurationDb.ClientPostLogoutRedirectUris.Where(s => s.ClientId == client.Id).ToListAsync();
            client.AllowedScopes =
                await _configurationDb.ClientScopes.Where(s => s.ClientId == client.Id).ToListAsync();
            client.IdentityProviderRestrictions =
                await _configurationDb.ClientIdPRestrictions.Where(s => s.ClientId == client.Id).ToListAsync();
            client.Claims =
                await _configurationDb.ClientClaims.Where(s => s.ClientId == client.Id).ToListAsync();
            client.AllowedCorsOrigins =
                await _configurationDb.ClientCorsOrigins.Where(s => s.ClientId == client.Id).ToListAsync();
            client.Properties =
                await _configurationDb.ClientProperties.Where(s => s.ClientId == client.Id).ToListAsync();

            var vm = _mapper.Map<VmClient>(client);
            return ApiResult.Success(vm);
        }
    }
}