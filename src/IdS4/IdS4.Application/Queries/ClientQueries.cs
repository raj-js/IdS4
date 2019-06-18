using AutoMapper;
using IdS4.Application.Models.Client;
using IdS4.Application.Models.Paging;
using IdS4.DbContexts;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;

namespace IdS4.Application.Queries
{
    public class ClientQueries: IClientQueries
    {
        private readonly IdS4ConfigurationDbContext _configurationDb;
        private readonly IMapper _mapper;

        public ClientQueries(IdS4ConfigurationDbContext configurationDb, IMapper mapper)
        {
            _configurationDb = configurationDb;
            _mapper = mapper;
        }

        public async Task<Paged<VmClient>> GetClients(PagingQuery query)
        {
            var clients = await _configurationDb.Clients
                .AsNoTracking()
                .OrderBy(query.Sort ?? "Id")
                .Skip(query.Skip)
                .Take(query.Limit)
                .ToListAsync();

            return Paged<VmClient>.From(
                _mapper.Map<List<VmClient>>(clients),
                await _configurationDb.Clients.AsNoTracking().CountAsync()
            );
        }

        public async Task<VmSplitClient> GetClient(int id)
        {
            var client = await _configurationDb.Clients.AsNoTracking().SingleOrDefaultAsync(s => s.Id == id);
            if (client == null) return null;

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
            return new VmSplitClient(_mapper, vm);
        }
    }
}
