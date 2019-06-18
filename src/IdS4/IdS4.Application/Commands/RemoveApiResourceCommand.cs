using MediatR;

namespace IdS4.Application.Commands
{
    public class RemoveApiResourceCommand: IRequest<bool>
    {
        public string ResourceIds { get; private set; }

        public RemoveApiResourceCommand(string resourceIds)
        {
            ResourceIds = resourceIds;
        }
    }
}
