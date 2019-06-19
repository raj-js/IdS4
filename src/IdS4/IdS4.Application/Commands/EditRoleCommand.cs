using IdS4.Application.Models.Role;
using MediatR;

namespace IdS4.Application.Commands
{
    public class EditRoleCommand : IRequest<VmRole>
    {
        public VmRole Role { get; private set; }

        public EditRoleCommand(VmRole role)
        {
            Role = role;
        }
    }
}
