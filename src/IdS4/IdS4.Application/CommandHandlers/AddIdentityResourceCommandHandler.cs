using AutoMapper;
using IdentityServer4.EntityFramework.Entities;
using IdS4.Application.Commands;
using IdS4.Application.Models.Resource;
using IdS4.DbContexts;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;

namespace IdS4.Application.CommandHandlers
{
    public class AddIdentityResourceCommandHandler:
        IRequestHandler<AddIdentityResourceCommand, VmIdentityResource>
    {
        private readonly IdS4ConfigurationDbContext _configurationDb;
        private readonly ILogger<AddIdentityResourceCommandHandler> _logger;
        private readonly IMapper _mapper;

        public AddIdentityResourceCommandHandler(IdS4ConfigurationDbContext configurationDb, ILogger<AddIdentityResourceCommandHandler> logger, IMapper mapper)
        {
            _configurationDb = configurationDb;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<VmIdentityResource> Handle(AddIdentityResourceCommand request, CancellationToken cancellationToken)
        {
            var resource = _mapper.Map<IdentityResource>(request.Resource);

            var entry = await _configurationDb.IdentityResources.AddAsync(resource, cancellationToken);
            await _configurationDb.SaveChangesAsync(cancellationToken);

            return _mapper.Map<VmIdentityResource>(entry.Entity);
        }
    }
}
