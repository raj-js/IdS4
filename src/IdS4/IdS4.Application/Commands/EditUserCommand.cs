using IdS4.Application.Models.User;
using MediatR;
using System;

namespace IdS4.Application.Commands
{
    public class EditUserCommand: IRequest<VmUser>
    {
        public VmUser User { get; private set; }

        public EditUserCommand(VmUser user)
        {
            User = user ?? throw new ArgumentNullException(nameof(user));
        }
    }
}
