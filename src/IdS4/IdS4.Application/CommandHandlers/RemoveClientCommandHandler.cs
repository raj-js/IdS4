using AutoMapper;
using IdS4.Application.Commands;
using IdS4.DbContexts;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace IdS4.Application.CommandHandlers
{
    public class RemoveClientCommandHandler: IRequestHandler<RemoveClientCommand, bool>
    {
        private readonly IdS4ConfigurationDbContext _configurationDb;
        private readonly IMapper _mapper;

        public RemoveClientCommandHandler(IdS4ConfigurationDbContext configurationDb, IMapper mapper)
        {
            _configurationDb = configurationDb;
            _mapper = mapper;
        }

        public async Task<bool> Handle(RemoveClientCommand request, CancellationToken cancellationToken)
        {
            foreach (var clientId in request.ClientIds.Split(','))
            {
                var client = await _configurationDb.Clients.FindAsync(new object[] { int.Parse(clientId) }, cancellationToken: cancellationToken);
                if (client == null) continue;

                _configurationDb.Clients.Remove(client);
            }
            return await _configurationDb.SaveChangesAsync(cancellationToken) > 0;
        }
    }
}
