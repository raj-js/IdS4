using AutoMapper;
using IdentityServer4.EntityFramework.Entities;
using IdS4.Application.Commands;
using IdS4.Application.Models.Resource;
using IdS4.DbContexts;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace IdS4.Application.CommandHandlers
{
    public class EditIdentityResourceCommandHandler :
        IRequestHandler<EditIdentityResourceCommand, VmIdentityResource>
    {
        private readonly IdS4ConfigurationDbContext _configurationDb;
        private readonly ILogger<AddIdentityResourceCommandHandler> _logger;
        private readonly IMapper _mapper;

        public EditIdentityResourceCommandHandler(IdS4ConfigurationDbContext configurationDb, ILogger<AddIdentityResourceCommandHandler> logger, IMapper mapper)
        {
            _configurationDb = configurationDb;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<VmIdentityResource> Handle(EditIdentityResourceCommand request, CancellationToken cancellationToken)
        {
            if (!await _configurationDb.IdentityResources
                .AsNoTracking()
                .AnyAsync(s => s.Id == request.Resource.Id, cancellationToken: cancellationToken))
                return null;

            await MarkUserClaimsDeleted(request.Resource.Id, request.Resource.UserClaims, cancellationToken);
            await MarkPropertiesDeleted(request.Resource.Id, request.Resource.Properties, cancellationToken);

            var resource = _mapper.Map<IdentityResource>(request.Resource);
            var entry = _configurationDb.Attach(resource);
            entry.State = EntityState.Modified;
            await _configurationDb.SaveChangesAsync(cancellationToken);

            return _mapper.Map<VmIdentityResource>(entry.Entity);
        }

        private async Task MarkUserClaimsDeleted(int resourceId, List<VmIdentityClaim> changed, CancellationToken cancellationToken = default)
        {
            var origin = await _configurationDb.Set<IdentityClaim>()
                .AsNoTracking()
                .Where(s => s.IdentityResourceId == resourceId)
                .ToListAsync(cancellationToken);

            var deleted = origin.Where(s => changed.All(c => c.Id != s.Id));

            foreach (var item in deleted)
                _configurationDb.Attach(item).State = EntityState.Deleted;
        }

        private async Task MarkPropertiesDeleted(int resourceId, List<VmIdentityResourceProperty> changed, CancellationToken cancellationToken = default)
        {
            var origin = await _configurationDb.Set<IdentityResourceProperty>()
                .AsNoTracking()
                .Where(s => s.IdentityResourceId == resourceId)
                .ToListAsync(cancellationToken);

            var deleted = origin.Where(s => changed.All(c => c.Id != s.Id));

            foreach (var item in deleted)
                _configurationDb.Attach(item).State = EntityState.Deleted;
        }
    }
}
