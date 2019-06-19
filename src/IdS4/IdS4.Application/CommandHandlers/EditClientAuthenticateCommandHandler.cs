using AutoMapper;
using IdentityServer4.EntityFramework.Entities;
using IdS4.Application.Commands;
using IdS4.Application.Models.Client;
using IdS4.DbContexts;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace IdS4.Application.CommandHandlers
{
    public class EditClientAuthenticateCommandHandler : IRequestHandler<EditClientAuthenticateCommand, VmClient.Authenticate>
    {
        private readonly IdS4ConfigurationDbContext _configurationDb;
        private readonly IMapper _mapper;

        public EditClientAuthenticateCommandHandler(IdS4ConfigurationDbContext configurationDb, IMapper mapper)
        {
            _configurationDb = configurationDb;
            _mapper = mapper;
        }

        public async Task<VmClient.Authenticate> Handle(EditClientAuthenticateCommand request, CancellationToken cancellationToken)
        {
            var client = await _configurationDb.Clients.AsNoTracking().SingleOrDefaultAsync(s => s.Id == request.Authenticate.Id, cancellationToken);
            if (client == null) return null;

            client.PostLogoutRedirectUris = await _configurationDb.ClientPostLogoutRedirectUris
                .AsNoTracking()
                .Where(s => s.ClientId == request.Authenticate.Id)
                .ToListAsync(cancellationToken);

            client.IdentityProviderRestrictions = await _configurationDb.ClientIdPRestrictions
                    .AsNoTracking()
                    .Where(s => s.ClientId == request.Authenticate.Id)
                    .ToListAsync(cancellationToken);
           
            var vm = _mapper.Map<VmClient>(client);
            request.Authenticate.ApplyChangeToClient(vm);

            await MarkPostLogoutRedirectUrisDeleted(vm.Id, vm.PostLogoutRedirectUris, cancellationToken);
            await MarkIdentityProviderRestrictionsDeleted(vm.Id, vm.IdentityProviderRestrictions, cancellationToken);

            var entity = _mapper.Map<Client>(vm);
            var entry = _configurationDb.Attach(entity);
            entry.State = EntityState.Modified;
            await _configurationDb.SaveChangesAsync();

            vm = _mapper.Map<VmClient>(entity);
            return vm.ToAuthenticate(_mapper);
        }

        private async Task MarkPostLogoutRedirectUrisDeleted(int clientId, List<VmClientPostLogoutRedirectUri> changed, CancellationToken cancellationToken = default)
        {
            var origin = await _configurationDb.Set<ClientPostLogoutRedirectUri>()
                .AsNoTracking()
                .Where(s => s.ClientId == clientId)
                .ToListAsync(cancellationToken);

            var deleted = origin.Where(s => changed.All(c => c.Id != s.Id));

            foreach (var item in deleted)
                _configurationDb.Attach(item).State = EntityState.Deleted;
        }

        private async Task MarkIdentityProviderRestrictionsDeleted(int clientId, List<VmClientIdPRestriction> changed, CancellationToken cancellationToken = default)
        {
            var origin = await _configurationDb.Set<ClientIdPRestriction>()
                .AsNoTracking()
                .Where(s => s.ClientId == clientId)
                .ToListAsync(cancellationToken);

            var deleted = origin.Where(s => changed.All(c => c.Id != s.Id));

            foreach (var item in deleted)
                _configurationDb.Attach(item).State = EntityState.Deleted;
        }
    }
}
