using AutoMapper;
using IdS4.Application.Commands;
using IdS4.DbContexts;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace IdS4.Application.CommandHandlers
{
    public class RemoveApiResourceCommandHandler :
        IRequestHandler<RemoveApiResourceCommand, bool>
    {
        private readonly IdS4ConfigurationDbContext _configurationDb;
        private readonly IMapper _mapper;

        public RemoveApiResourceCommandHandler(IdS4ConfigurationDbContext configurationDb, IMapper mapper)
        {
            _configurationDb = configurationDb;
            _mapper = mapper;
        }

        public async Task<bool> Handle(RemoveApiResourceCommand request, CancellationToken cancellationToken)
        {
            foreach (var resourceId in request.ResourceIds.Split(','))
            {
                var resource = await _configurationDb.ApiResources.FindAsync(new object[] { int.Parse(resourceId) }, cancellationToken: cancellationToken);
                if (resource == null) continue;

                _configurationDb.ApiResources.Remove(resource);
            }
            return await _configurationDb.SaveChangesAsync(cancellationToken) > 0;
        }
    }
}
