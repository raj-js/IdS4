using System;
using IdS4.Application.Models.Resource;
using MediatR;

namespace IdS4.Application.Commands
{
    public class AddIdentityResourceCommand: IRequest<VmIdentityResource>
    {
        public VmIdentityResource Resource { get; private set; }

        public AddIdentityResourceCommand(VmIdentityResource resource)
        {
            Resource = resource ?? throw new ArgumentNullException(nameof(resource));
        }
    }
}
