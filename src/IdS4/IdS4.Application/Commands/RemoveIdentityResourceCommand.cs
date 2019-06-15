using MediatR;

namespace IdS4.Application.Commands
{
    public class RemoveIdentityResourceCommand : IRequest<bool>
    {
        public string ResourceIds { get; private set; }

        public RemoveIdentityResourceCommand(string resourceIds)
        {
            ResourceIds = resourceIds;
        }
    }
}
