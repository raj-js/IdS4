using MediatR;

namespace IdS4.Application.Commands
{
    public class RemoveRoleCommand : IRequest<bool>
    {
        public string RoleIds { get; private set; }

        public RemoveRoleCommand(string roleIds)
        {
            RoleIds = roleIds;
        }
    }
}
