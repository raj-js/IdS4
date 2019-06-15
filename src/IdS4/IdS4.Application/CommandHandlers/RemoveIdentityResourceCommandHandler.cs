using AutoMapper;
using IdS4.Application.Commands;
using IdS4.DbContexts;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;

namespace IdS4.Application.CommandHandlers
{
    public class RemoveIdentityResourceCommandHandler :
        IRequestHandler<RemoveIdentityResourceCommand, bool>
    {
        private readonly IdS4ConfigurationDbContext _configurationDb;
        private readonly ILogger<AddIdentityResourceCommandHandler> _logger;
        private readonly IMapper _mapper;

        public RemoveIdentityResourceCommandHandler(IdS4ConfigurationDbContext configurationDb, ILogger<AddIdentityResourceCommandHandler> logger, IMapper mapper)
        {
            _configurationDb = configurationDb;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<bool> Handle(RemoveIdentityResourceCommand request, CancellationToken cancellationToken)
        {

            foreach (var resourceId in request.ResourceIds.Split(','))
            {
                var resource = await _configurationDb.IdentityResources.FindAsync(int.Parse(resourceId));
                if (resource == null) continue;

                _configurationDb.IdentityResources.Remove(resource);
            }
            return await _configurationDb.SaveChangesAsync() > 0;
        }
    }
}
