using System;
using IdS4.Application.Models.Resource;
using MediatR;

namespace IdS4.Application.Commands
{
    public class AddApiResourceCommand: IRequest<VmApiResource>
    {
        public VmApiResource Resource { get; private set; }

        public AddApiResourceCommand(VmApiResource resource)
        {
            Resource = resource ?? throw new ArgumentNullException(nameof(resource));
        }
    }
}
