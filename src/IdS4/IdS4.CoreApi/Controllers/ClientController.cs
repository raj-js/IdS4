using AutoMapper;
using IdentityServer4.EntityFramework.Entities;
using IdS4.Application.Models.Client;
using IdS4.Application.Models.Paging;
using IdS4.CoreApi.Extensions;
using IdS4.CoreApi.Models.Results;
using IdS4.DbContexts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using IdS4.Application.Queries;
using GrantType = IdentityServer4.Models.GrantType;

namespace IdS4.CoreApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [BearerAuthorize]
    public class ClientController : ControllerBase
    {
        private readonly IdS4ConfigurationDbContext _configurationDb;
        private readonly ILogger<ScopeController> _logger;
        private readonly IMapper _mapper;
        private readonly IClientQueries _clientQueries;

        public ClientController(IdS4ConfigurationDbContext configurationDb, ILogger<ScopeController> logger, IMapper mapper, IClientQueries clientQueries)
        {
            _configurationDb = configurationDb;
            _logger = logger;
            _mapper = mapper;
            _clientQueries = clientQueries;
        }

        [HttpGet]
        public async Task<Paged<VmClient>> Get([FromQuery]PagingQuery query)
        {
            return await _clientQueries.GetClients(query);
        }

        [HttpGet("{id}")]
        public async Task<ApiResult> Get([FromRoute]int id)
        {
            if (id <= 0) return ApiResult.NotFound(id);

            var vmSplitClient = await _clientQueries.GetClient(id);
            if (vmSplitClient == null)
                return ApiResult.NotFound(id);

            return ApiResult.Success(vmSplitClient);
        }

        [HttpPost]
        public async Task<ApiResult> Add([FromBody]VmClientAdd vm)
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

        [HttpPatch("basic")]
        public async Task<ApiResult> Edit([FromBody]VmClient.Basic vm)
        {
            if (vm.Id <= 0) return ApiResult.NotFound(vm.Id);

            var apiResult = await GetClient(vm.Id);
            if (apiResult.Code != ApiResultCode.Success) return apiResult;

            var vmClient = (apiResult as ApiResult<VmClient>)?.Data;
            if (vmClient == null) return ApiResult.Failure();

            vm.ApplyChangeToClient(vmClient);

            await MarkClientSecretsDeleted(vm.Id, vm.ClientSecrets);
            await MarkAllowedGrantTypesDeleted(vm.Id, vm.AllowedGrantTypes);
            await MarkRedirectUrisDeleted(vm.Id, vm.RedirectUris);
            await MarkAllowedScopesDeleted(vm.Id, vm.AllowedScopes);
            await MarkPropertiesDeleted(vm.Id, vm.Properties);

            var client = await EditClient(vmClient);
            await _configurationDb.SaveChangesAsync();

            vmClient = _mapper.Map<VmClient>(client);
            return ApiResult.Success(vmClient.ToBasic(_mapper));
        }

        [HttpPatch("authenticate")]
        public async Task<ApiResult> Edit([FromBody]VmClient.Authenticate vm)
        {
            if (vm.Id <= 0) return ApiResult.NotFound(vm.Id);

            var apiResult = await GetClient(vm.Id);
            if (apiResult.Code != ApiResultCode.Success) return apiResult;

            var vmClient = (apiResult as ApiResult<VmClient>)?.Data;
            if (vmClient == null) return ApiResult.Failure();

            vm.ApplyChangeToClient(vmClient);

            await MarkPostLogoutRedirectUrisDeleted(vm.Id, vm.PostLogoutRedirectUris);
            await MarkIdentityProviderRestrictionsDeleted(vm.Id, vm.IdentityProviderRestrictions);

            var client = await EditClient(vmClient);
            await _configurationDb.SaveChangesAsync();

            vmClient = _mapper.Map<VmClient>(client);
            return ApiResult.Success(vmClient.ToAuthenticate(_mapper));
        }

        [HttpPatch("token")]
        public async Task<ApiResult> Edit([FromBody]VmClient.Token vm)
        {
            if (vm.Id <= 0) return ApiResult.NotFound(vm.Id);

            var apiResult = await GetClient(vm.Id);
            if (apiResult.Code != ApiResultCode.Success) return apiResult;

            var vmClient = (apiResult as ApiResult<VmClient>)?.Data;
            if (vmClient == null) return ApiResult.Failure();

            vm.ApplyChangeToClient(vmClient);

            await MarkAllowedCorsOriginsDeleted(vm.Id, vm.AllowedCorsOrigins);
            await MarkClaimsDeleted(vm.Id, vm.Claims);

            var client = await EditClient(vmClient);
            await _configurationDb.SaveChangesAsync();

            vmClient = _mapper.Map<VmClient>(client);
            return ApiResult.Success(vmClient.ToToken(_mapper));
        }

        [HttpPatch("consent")]
        public async Task<ApiResult> Edit([FromBody]VmClient.Consent vm)
        {
            if (vm.Id <= 0) return ApiResult.NotFound(vm.Id);

            var apiResult = await GetClient(vm.Id);
            if (apiResult.Code != ApiResultCode.Success) return apiResult;

            var vmClient = (apiResult as ApiResult<VmClient>)?.Data;
            if (vmClient == null) return ApiResult.Failure();

            vm.ApplyChangeToClient(vmClient);
            var client = await EditClient(vmClient);
            await _configurationDb.SaveChangesAsync();

            vmClient = _mapper.Map<VmClient>(client);
            return ApiResult.Success(vmClient.ToConsent(_mapper));
        }

        [HttpPatch("device")]
        public async Task<ApiResult> Edit([FromBody]VmClient.Device vm)
        {
            if (vm.Id <= 0) return ApiResult.NotFound(vm.Id);

            var apiResult = await GetClient(vm.Id);
            if (apiResult.Code != ApiResultCode.Success) return apiResult;

            var vmClient = (apiResult as ApiResult<VmClient>)?.Data;
            if (vmClient == null) return ApiResult.Failure();

            vm.ApplyChangeToClient(vmClient);
            var client = await EditClient(vmClient);
            await _configurationDb.SaveChangesAsync();

            vmClient = _mapper.Map<VmClient>(client);
            return ApiResult.Success(vmClient.ToDevice(_mapper));
        }

        [HttpDelete("{ids}")]
        public async Task<ApiResult> Remove([FromRoute]string ids)
        {
            if (string.IsNullOrEmpty(ids)) return ApiResult.Failure();

            foreach (var id in ids.Split(","))
            {
                var entity = await _configurationDb.Clients.FindAsync(int.Parse(id));
                if (entity == null) continue;

                _configurationDb.Clients.Remove(entity);
            }
            await _configurationDb.SaveChangesAsync();
            return ApiResult.Success();
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

        private async Task<ApiResult> GetClient(int id)
        {
            var client = await _configurationDb.Clients.AsNoTracking().SingleOrDefaultAsync(s => s.Id == id);
            if (client == null) return ApiResult.NotFound(id);

            client.ClientSecrets =
                await _configurationDb.ClientSecrets.AsNoTracking().Where(s => s.ClientId == client.Id).ToListAsync();
            client.AllowedGrantTypes =
                await _configurationDb.ClientGrantTypes.AsNoTracking().Where(s => s.ClientId == client.Id).ToListAsync();
            client.RedirectUris =
                await _configurationDb.ClientRedirectUris.AsNoTracking().Where(s => s.ClientId == client.Id).ToListAsync();
            client.PostLogoutRedirectUris =
                await _configurationDb.ClientPostLogoutRedirectUris.AsNoTracking().Where(s => s.ClientId == client.Id).ToListAsync();
            client.AllowedScopes =
                await _configurationDb.ClientScopes.AsNoTracking().Where(s => s.ClientId == client.Id).ToListAsync();
            client.IdentityProviderRestrictions =
                await _configurationDb.ClientIdPRestrictions.AsNoTracking().Where(s => s.ClientId == client.Id).ToListAsync();
            client.Claims =
                await _configurationDb.ClientClaims.AsNoTracking().Where(s => s.ClientId == client.Id).ToListAsync();
            client.AllowedCorsOrigins =
                await _configurationDb.ClientCorsOrigins.AsNoTracking().Where(s => s.ClientId == client.Id).ToListAsync();
            client.Properties =
                await _configurationDb.ClientProperties.AsNoTracking().Where(s => s.ClientId == client.Id).ToListAsync();

            var vm = _mapper.Map<VmClient>(client);
            return ApiResult.Success(vm);
        }

        private async Task<Client> EditClient(VmClient vm)
        {
            var entity = _mapper.Map<Client>(vm);
            var entry = _configurationDb.Attach(entity);
            entry.State = EntityState.Modified;

            await Task.CompletedTask;
            return entity;
        }

        private async Task MarkClientSecretsDeleted(int clientId, List<VmClientSecret> changed)
        {
            var origin = await _configurationDb.Set<ClientSecret>()
                .AsNoTracking()
                .Where(s => s.ClientId == clientId)
                .ToListAsync();

            var deleted = origin.Where(s => changed.All(c => c.Id != s.Id));

            foreach (var item in deleted)
                _configurationDb.Attach(item).State = EntityState.Deleted;
        }

        private async Task MarkAllowedGrantTypesDeleted(int clientId, List<VmClientGrantType> changed)
        {
            var origin = await _configurationDb.Set<ClientGrantType>()
                .AsNoTracking()
                .Where(s => s.ClientId == clientId)
                .ToListAsync();

            var deleted = origin.Where(s => changed.All(c => c.Id != s.Id));

            foreach (var item in deleted)
                _configurationDb.Attach(item).State = EntityState.Deleted;
        }

        private async Task MarkRedirectUrisDeleted(int clientId, List<VmClientRedirectUri> changed)
        {
            var origin = await _configurationDb.Set<ClientRedirectUri>()
                .AsNoTracking()
                .Where(s => s.ClientId == clientId)
                .ToListAsync();

            var deleted = origin.Where(s => changed.All(c => c.Id != s.Id));

            foreach (var item in deleted)
                _configurationDb.Attach(item).State = EntityState.Deleted;
        }

        private async Task MarkAllowedScopesDeleted(int clientId, List<VmClientScope> changed)
        {
            var origin = await _configurationDb.Set<ClientScope>()
                .AsNoTracking()
                .Where(s => s.ClientId == clientId)
                .ToListAsync();

            var deleted = origin.Where(s => changed.All(c => c.Id != s.Id));

            foreach (var item in deleted)
                _configurationDb.Attach(item).State = EntityState.Deleted;
        }

        private async Task MarkPropertiesDeleted(int clientId, List<VmClientProperty> changed)
        {
            var origin = await _configurationDb.Set<ClientProperty>()
                .AsNoTracking()
                .Where(s => s.ClientId == clientId)
                .ToListAsync();

            var deleted = origin.Where(s => changed.All(c => c.Id != s.Id));

            foreach (var item in deleted)
                _configurationDb.Attach(item).State = EntityState.Deleted;
        }

        private async Task MarkPostLogoutRedirectUrisDeleted(int clientId, List<VmClientPostLogoutRedirectUri> changed)
        {
            var origin = await _configurationDb.Set<ClientPostLogoutRedirectUri>()
                .AsNoTracking()
                .Where(s => s.ClientId == clientId)
                .ToListAsync();

            var deleted = origin.Where(s => changed.All(c => c.Id != s.Id));

            foreach (var item in deleted)
                _configurationDb.Attach(item).State = EntityState.Deleted;
        }

        private async Task MarkIdentityProviderRestrictionsDeleted(int clientId, List<VmClientIdPRestriction> changed)
        {
            var origin = await _configurationDb.Set<ClientIdPRestriction>()
                .AsNoTracking()
                .Where(s => s.ClientId == clientId)
                .ToListAsync();

            var deleted = origin.Where(s => changed.All(c => c.Id != s.Id));

            foreach (var item in deleted)
                _configurationDb.Attach(item).State = EntityState.Deleted;
        }

        private async Task MarkAllowedCorsOriginsDeleted(int clientId, List<VmClientCorsOrigin> changed)
        {
            var origin = await _configurationDb.Set<ClientCorsOrigin>()
                .AsNoTracking()
                .Where(s => s.ClientId == clientId)
                .ToListAsync();

            var deleted = origin.Where(s => changed.All(c => c.Id != s.Id));

            foreach (var item in deleted)
                _configurationDb.Attach(item).State = EntityState.Deleted;
        }

        private async Task MarkClaimsDeleted(int clientId, List<VmClientClaim> changed)
        {
            var origin = await _configurationDb.Set<ClientClaim>()
                .AsNoTracking()
                .Where(s => s.ClientId == clientId)
                .ToListAsync();

            var deleted = origin.Where(s => changed.All(c => c.Id != s.Id));

            foreach (var item in deleted)
                _configurationDb.Attach(item).State = EntityState.Deleted;
        }

        #endregion
    }
}