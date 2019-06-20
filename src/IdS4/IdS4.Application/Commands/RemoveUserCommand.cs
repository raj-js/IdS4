using MediatR;

namespace IdS4.Application.Commands
{
    public class RemoveUserCommand : IRequest<bool>
    {
        public string UserIds { get; private set; }

        public RemoveUserCommand(string userIds)
        {
            UserIds = userIds;
        }
    }
}
