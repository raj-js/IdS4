using IdS4.Application.Models.Resource;
using MediatR;
using System;

namespace IdS4.Application.Commands
{
    public class EditIdentityResourceCommand : IRequest<VmIdentityResource>
    {
        public VmIdentityResource Resource { get; private set; }

        public EditIdentityResourceCommand(VmIdentityResource resource)
        {
            Resource = resource ?? throw new ArgumentNullException(nameof(resource));
        }
    }
}
