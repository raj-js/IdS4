using IdS4.CoreApi.Models.Role;
using MediatR;

namespace IdS4.Application.Commands
{
    public class AddRoleCommand : IRequest<VmRole>
    {
        public VmRole Role { get; private set; }

        public AddRoleCommand(VmRole role)
        {
            Role = role;
        }
    }
}
