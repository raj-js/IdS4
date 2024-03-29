﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using IdentityServer4.EntityFramework.Entities;
using IdS4.Application.Commands;
using IdS4.Application.Models.Client;
using IdS4.DbContexts;
using MediatR;
using Microsoft.EntityFrameworkCore;
 
namespace IdS4.Application.CommandHandlers
{
    public class EditClientTokenCommandHandler : IRequestHandler<EditClientTokenCommand, VmClient.Token>
    {
        private readonly IdS4ConfigurationDbContext _configurationDb;
        private readonly IMapper _mapper;

        public EditClientTokenCommandHandler(IdS4ConfigurationDbContext configurationDb, IMapper mapper)
        {
            _configurationDb = configurationDb;
            _mapper = mapper;
        }

        public async Task<VmClient.Token> Handle(EditClientTokenCommand request, CancellationToken cancellationToken)
        {
            var client = await _configurationDb.Clients.AsNoTracking().SingleOrDefaultAsync(s => s.Id == request.Token.Id, cancellationToken);
            if (client == null) return null;

            client.AllowedCorsOrigins = await _configurationDb.ClientCorsOrigins
                .AsNoTracking()
                .Where(s => s.ClientId == request.Token.Id)
                .ToListAsync(cancellationToken);

            client.Claims = await _configurationDb.ClientClaims
                .AsNoTracking()
                .Where(s => s.ClientId == request.Token.Id)
                .ToListAsync(cancellationToken);

            var vm = _mapper.Map<VmClient>(client);
            request.Token.ApplyChangeToClient(vm);

            await MarkAllowedCorsOriginsDeleted(vm.Id, vm.AllowedCorsOrigins, cancellationToken);
            await MarkClaimsDeleted(vm.Id, vm.Claims, cancellationToken);

            var entity = _mapper.Map<Client>(vm);
            var entry = _configurationDb.Attach(entity);
            entry.State = EntityState.Modified;
            await _configurationDb.SaveChangesAsync(cancellationToken);

            vm = _mapper.Map<VmClient>(entity);
            return vm.ToToken(_mapper);
        }

        private async Task MarkAllowedCorsOriginsDeleted(int clientId, List<VmClientCorsOrigin> changed, CancellationToken cancellationToken = default)
        {
            var origin = await _configurationDb.Set<ClientCorsOrigin>()
                .AsNoTracking()
                .Where(s => s.ClientId == clientId)
                .ToListAsync(cancellationToken);

            var deleted = origin.Where(s => changed.All(c => c.Id != s.Id));

            foreach (var item in deleted)
                _configurationDb.Attach(item).State = EntityState.Deleted;
        }

        private async Task MarkClaimsDeleted(int clientId, List<VmClientClaim> changed, CancellationToken cancellationToken = default)
        {
            var origin = await _configurationDb.Set<ClientClaim>()
                .AsNoTracking()
                .Where(s => s.ClientId == clientId)
                .ToListAsync(cancellationToken);

            var deleted = origin.Where(s => changed.All(c => c.Id != s.Id));

            foreach (var item in deleted)
                _configurationDb.Attach(item).State = EntityState.Deleted;
        }
    }
}
