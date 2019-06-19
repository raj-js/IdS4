using AutoMapper;
using IdentityServer4.EntityFramework.Entities;
using IdS4.Application.Commands;
using IdS4.Application.Models.Client;
using IdS4.DbContexts;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;
using GrantType = IdentityServer4.Models.GrantType;

namespace IdS4.Application.CommandHandlers
{
    public class AddClientCommandHandler : IRequestHandler<AddClientCommand, VmClient>
    {
        private readonly IdS4ConfigurationDbContext _configurationDb;
        private readonly IMapper _mapper;

        public AddClientCommandHandler(IdS4ConfigurationDbContext configurationDb, IMapper mapper)
        {
            _configurationDb = configurationDb;
            _mapper = mapper;
        }

        public async Task<VmClient> Handle(AddClientCommand request, CancellationToken cancellationToken)
        {
            var vmClient = PrepareClient(request.Client);

            if (await _configurationDb.Clients.AsNoTracking()
                .AnyAsync(s => s.ClientId.Equals(vmClient.ClientId), cancellationToken))
                return null;

            var client = _mapper.Map<Client>(vmClient);
            await _configurationDb.Clients.AddAsync(client, cancellationToken);
            await _configurationDb.SaveChangesAsync(cancellationToken);

            return _mapper.Map<VmClient>(client);
        }

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
    }
}
