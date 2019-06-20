using IdS4.Application.Models.User;
using MediatR;
using System;

namespace IdS4.Application.Commands
{
    public class AddUserCommand: IRequest<VmUserAdd>
    {
        public VmUser User { get; private set; }

        public AddUserCommand(VmUser user)
        {
            User = user ?? throw new ArgumentNullException(nameof(user));
        }
    }
}
