using AutoMapper;
using IdentityServer4.EntityFramework.Entities;
using IdS4.Application.Commands;
using IdS4.Application.Models.Resource;
using IdS4.DbContexts;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace IdS4.Application.CommandHandlers
{
    public class EditApiResourceCommandHandler:
        IRequestHandler<EditApiResourceCommand, VmApiResource>
    {
        private readonly IdS4ConfigurationDbContext _configurationDb;
        private readonly IMapper _mapper;

        public EditApiResourceCommandHandler(IdS4ConfigurationDbContext configurationDb, IMapper mapper)
        {
            _configurationDb = configurationDb;
            _mapper = mapper;
        }

        public async Task<VmApiResource> Handle(EditApiResourceCommand request, CancellationToken cancellationToken)
        {
            if (!await _configurationDb.ApiResources
                .AsNoTracking()
                .AnyAsync(s => s.Id == request.Resource.Id, cancellationToken: cancellationToken))
                return null;

            await MarkUserClaimsDeleted(request.Resource.Id, request.Resource.UserClaims, cancellationToken);
            await MarkScopesDeleted(request.Resource.Id, request.Resource.Scopes, cancellationToken);
            await MarkSecretsDeleted(request.Resource.Id, request.Resource.Secrets, cancellationToken);
            await MarkPropertiesDeleted(request.Resource.Id, request.Resource.Properties, cancellationToken);

            var resource = _mapper.Map<ApiResource>(request.Resource);
            resource.Updated = DateTime.Now;

            var entry = _configurationDb.Attach(resource);
            entry.State = EntityState.Modified;
            await _configurationDb.SaveChangesAsync(cancellationToken);

            return _mapper.Map<VmApiResource>(entry.Entity);
        }

        private async Task MarkUserClaimsDeleted(int resourceId, List<VmApiResourceClaim> changed, CancellationToken cancellationToken = default)
        {
            var origin = await _configurationDb.Set<ApiResourceClaim>()
                .AsNoTracking()
                .Where(s => s.ApiResourceId == resourceId)
                .ToListAsync(cancellationToken);

            var deleted = origin.Where(s => changed.All(c => c.Id != s.Id));

            foreach (var item in deleted)
                _configurationDb.Attach(item).State = EntityState.Deleted;
        }

        private async Task MarkSecretsDeleted(int resourceId, List<VmApiSecret> changed, CancellationToken cancellationToken = default)
        {
            var origin = await _configurationDb.Set<ApiSecret>()
                .AsNoTracking()
                .Where(s => s.ApiResourceId == resourceId)
                .ToListAsync(cancellationToken);

            var deleted = origin.Where(s => changed.All(c => c.Id != s.Id));

            foreach (var item in deleted)
                _configurationDb.Attach(item).State = EntityState.Deleted;
        }

        private async Task MarkScopesDeleted(int resourceId, List<VmApiScope> changed, CancellationToken cancellationToken = default)
        {
            var origin = await _configurationDb.Set<ApiScope>()
                .AsNoTracking()
                .Where(s => s.ApiResourceId == resourceId)
                .ToListAsync(cancellationToken);

            var deleted = origin.Where(s => changed.All(c => c.Id != s.Id));

            foreach (var item in deleted)
                _configurationDb.Attach(item).State = EntityState.Deleted;
        }

        private async Task MarkPropertiesDeleted(int resourceId, List<VmApiResourceProperty> changed, CancellationToken cancellationToken = default)
        {
            var origin = await _configurationDb.Set<ApiResourceProperty>()
                .AsNoTracking()
                .Where(s => s.ApiResourceId == resourceId)
                .ToListAsync(cancellationToken);

            var deleted = origin.Where(s => changed.All(c => c.Id != s.Id));

            foreach (var item in deleted)
                _configurationDb.Attach(item).State = EntityState.Deleted;
        }
    }
}
