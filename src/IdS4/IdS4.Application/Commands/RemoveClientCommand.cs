using MediatR;

namespace IdS4.Application.Commands
{
    public class RemoveClientCommand : IRequest<bool>
    {
        public string ClientIds { get; private set; }

        public RemoveClientCommand(string clientIds)
        {
            ClientIds = clientIds;
        }
    }
}
