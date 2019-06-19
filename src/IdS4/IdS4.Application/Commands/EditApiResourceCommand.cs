using IdS4.Application.Models.Resource;
using MediatR;
using System;

namespace IdS4.Application.Commands
{
    public class EditApiResourceCommand: IRequest<VmApiResource>
    {
        public VmApiResource Resource { get; private set; }

        public EditApiResourceCommand(VmApiResource resource)
        {
            Resource = resource ?? throw new ArgumentNullException(nameof(resource));
        }
    }
}
