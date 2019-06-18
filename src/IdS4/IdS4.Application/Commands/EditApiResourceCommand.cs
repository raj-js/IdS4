using System;
using System.Collections.Generic;
using System.Text;
using IdS4.Application.Models.Resource;
using MediatR;

namespace IdS4.Application.Commands
{
    public class EditApiResourceCommand: IRequest<VmApiResource>
    {
        public VmIdentityResource Resource { get; private set; }

        public EditApiResourceCommand(VmIdentityResource resource)
        {
            Resource = resource ?? throw new ArgumentNullException(nameof(resource));
        }
    }
}
