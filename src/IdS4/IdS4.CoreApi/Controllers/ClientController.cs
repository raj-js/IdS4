using System;
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
using IdentityServer4.EntityFramework.Entities;
using GrantType = IdentityServer4.Models.GrantType;

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

        [HttpPost]
        public async Task<ApiResult> AddClient([FromBody]VmClientAdd vm)
        {
            if (!ModelState.IsValid) return ApiResult.Failure(ModelState);

            var vmClient = PrepareClient(vm);

            var apiResult = await ValidateClient(vmClient);
            if (apiResult.Code != ApiResultCode.Success) return apiResult;

            var client = _mapper.Map<Client>(vmClient);
            await _configurationDb.Clients.AddAsync(client);
            await _configurationDb.SaveChangesAsync();
            return ApiResult.Success(_mapper.Map<VmClient>(client));
        }

        [HttpPatch]
        public async Task<ApiResult> EditBasic()
        {
            return null;
        }

        #region privates

        private VmClient PrepareClient(VmClientAdd vm)
        {
            var client = new VmClient
            {
                ClientId = vm.ClientId,
                ClientName = vm.ClientName
            };

            switch (vm.Type)
            {
                case VmClientType.Empty:
                    break;
                case VmClientType.Hybrid:
                    client.AllowedGrantTypes.Add(new VmClientGrantType(GrantType.Hybrid));
                    break;
                case VmClientType.SPA:
                    client.AllowedGrantTypes.Add(new VmClientGrantType(GrantType.AuthorizationCode));
                    client.RequirePkce = true;
                    client.RequireClientSecret = false;
                    break;
                case VmClientType.Native:
                    client.AllowedGrantTypes.Add(new VmClientGrantType(GrantType.Hybrid));
                    break;
                case VmClientType.Machine:
                    client.AllowedGrantTypes.Add(new VmClientGrantType(GrantType.ResourceOwnerPassword));
                    client.AllowedGrantTypes.Add(new VmClientGrantType(GrantType.ClientCredentials));
                    break;
                case VmClientType.Device:
                    client.AllowedGrantTypes.Add(new VmClientGrantType(GrantType.DeviceFlow));
                    client.RequireClientSecret = false;
                    client.AllowOfflineAccess = true;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            return client;
        }

        private async Task<ApiResult> ValidateClient(VmClient client)
        {
            if (await _configurationDb.Clients.AnyAsync(s => s.ClientId.Equals(client.ClientId)))
            {
                return ApiResult.Failure(errors:
                    new KeyValuePair<string, object>("客户端已存在", $"客户端ID: {client.ClientId} 已存在")
                    );
            }

            return ApiResult.Success();
        }

        #endregion
    }
}