using System;
using IdS4.Application.Models.Role;
using MediatR;

namespace IdS4.Application.Commands
{
    public class AddRoleCommand : IRequest<VmRole>
    {
        public VmRole Role { get; private set; }

        public AddRoleCommand(VmRole role)
        {
            Role = role ?? throw new ArgumentNullException(nameof(role));
        }
    }
}
