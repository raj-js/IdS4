using AutoMapper;
using IdS4.Application.Commands;
using IdS4.DbContexts;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace IdS4.Application.CommandHandlers
{
    public class RemoveIdentityResourceCommandHandler :
        IRequestHandler<RemoveIdentityResourceCommand, bool>
    {
        private readonly IdS4ConfigurationDbContext _configurationDb;
        private readonly IMapper _mapper;

        public RemoveIdentityResourceCommandHandler(IdS4ConfigurationDbContext configurationDb, IMapper mapper)
        {
            _configurationDb = configurationDb;
            _mapper = mapper;
        }

        public async Task<bool> Handle(RemoveIdentityResourceCommand request, CancellationToken cancellationToken)
        {
            foreach (var resourceId in request.ResourceIds.Split(','))
            {
                var resource = await _configurationDb.IdentityResources.FindAsync(new object[]{ int.Parse(resourceId) }, cancellationToken: cancellationToken);
                if (resource == null) continue;

                _configurationDb.IdentityResources.Remove(resource);
            }
            return await _configurationDb.SaveChangesAsync(cancellationToken) > 0;
        }
    }
}
