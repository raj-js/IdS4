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
    public class EditClientBasicCommandHandler : IRequestHandler<EditClientBasicCommand, VmClient.Basic>
    {
        private readonly IdS4ConfigurationDbContext _configurationDb;
        private readonly IMapper _mapper;

        public EditClientBasicCommandHandler(IdS4ConfigurationDbContext configurationDb, IMapper mapper)
        {
            _configurationDb = configurationDb;
            _mapper = mapper;
        }

        public async Task<VmClient.Basic> Handle(EditClientBasicCommand request, CancellationToken cancellationToken)
        {
            var client = await _configurationDb.Clients.AsNoTracking().SingleOrDefaultAsync(s => s.Id == request.Basic.Id, cancellationToken);
            if (client == null) return null;

            client.ClientSecrets = await _configurationDb.ClientSecrets
                .AsNoTracking()
                .Where(s => s.ClientId == request.Basic.Id)
                .ToListAsync(cancellationToken);

            client.AllowedGrantTypes = await _configurationDb.ClientGrantTypes
                .AsNoTracking()
                .Where(s => s.ClientId == request.Basic.Id)
                .ToListAsync(cancellationToken);

            client.RedirectUris = await _configurationDb.ClientRedirectUris
                .AsNoTracking()
                .Where(s => s.ClientId == request.Basic.Id)
                .ToListAsync(cancellationToken);

            client.AllowedScopes = await _configurationDb.ClientScopes
                .AsNoTracking()
                .Where(s => s.ClientId == request.Basic.Id)
                .ToListAsync(cancellationToken);

            client.Properties = await _configurationDb.ClientProperties
                .AsNoTracking()
                .Where(s => s.ClientId == request.Basic.Id)
                .ToListAsync(cancellationToken);

            var vm = _mapper.Map<VmClient>(client);
            request.Basic.ApplyChangeToClient(vm);

            await MarkClientSecretsDeleted(vm.Id, vm.ClientSecrets, cancellationToken);
            await MarkAllowedGrantTypesDeleted(vm.Id, vm.AllowedGrantTypes, cancellationToken);
            await MarkRedirectUrisDeleted(vm.Id, vm.RedirectUris, cancellationToken);
            await MarkAllowedScopesDeleted(vm.Id, vm.AllowedScopes, cancellationToken);
            await MarkPropertiesDeleted(vm.Id, vm.Properties, cancellationToken);

            var entity = _mapper.Map<Client>(vm);
            var entry = _configurationDb.Attach(entity);
            entry.State = EntityState.Modified;
            await _configurationDb.SaveChangesAsync();

            vm = _mapper.Map<VmClient>(entity);
            return vm.ToBasic(_mapper);
        }

        private async Task MarkClientSecretsDeleted(int clientId, List<VmClientSecret> changed, CancellationToken cancellationToken = default)
        {
            var origin = await _configurationDb.Set<ClientSecret>()
                .AsNoTracking()
                .Where(s => s.ClientId == clientId)
                .ToListAsync(cancellationToken);

            var deleted = origin.Where(s => changed.All(c => c.Id != s.Id));

            foreach (var item in deleted)
                _configurationDb.Attach(item).State = EntityState.Deleted;
        }

        private async Task MarkAllowedGrantTypesDeleted(int clientId, List<VmClientGrantType> changed, CancellationToken cancellationToken = default)
        {
            var origin = await _configurationDb.Set<ClientGrantType>()
                .AsNoTracking()
                .Where(s => s.ClientId == clientId)
                .ToListAsync(cancellationToken);

            var deleted = origin.Where(s => changed.All(c => c.Id != s.Id));

            foreach (var item in deleted)
                _configurationDb.Attach(item).State = EntityState.Deleted;
        }

        private async Task MarkRedirectUrisDeleted(int clientId, List<VmClientRedirectUri> changed, CancellationToken cancellationToken = default)
        {
            var origin = await _configurationDb.Set<ClientRedirectUri>()
                .AsNoTracking()
                .Where(s => s.ClientId == clientId)
                .ToListAsync(cancellationToken);

            var deleted = origin.Where(s => changed.All(c => c.Id != s.Id));

            foreach (var item in deleted)
                _configurationDb.Attach(item).State = EntityState.Deleted;
        }

        private async Task MarkAllowedScopesDeleted(int clientId, List<VmClientScope> changed, CancellationToken cancellationToken = default)
        {
            var origin = await _configurationDb.Set<ClientScope>()
                .AsNoTracking()
                .Where(s => s.ClientId == clientId)
                .ToListAsync(cancellationToken);

            var deleted = origin.Where(s => changed.All(c => c.Id != s.Id));

            foreach (var item in deleted)
                _configurationDb.Attach(item).State = EntityState.Deleted;
        }

        private async Task MarkPropertiesDeleted(int clientId, List<VmClientProperty> changed, CancellationToken cancellationToken = default)
        {
            var origin = await _configurationDb.Set<ClientProperty>()
                .AsNoTracking()
                .Where(s => s.ClientId == clientId)
                .ToListAsync(cancellationToken);

            var deleted = origin.Where(s => changed.All(c => c.Id != s.Id));

            foreach (var item in deleted)
                _configurationDb.Attach(item).State = EntityState.Deleted;
        }
    }
}
