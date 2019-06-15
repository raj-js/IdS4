using IdS4.Application.Commands;
using IdS4.Application.Models.Resource;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace IdS4.Application.CommandHandlers
{
    public class EditIdentityResourceCommandHandler :
        IRequestHandler<EditIdentityResourceCommand, VmIdentityResource>
    {
        public async Task<VmIdentityResource> Handle(EditIdentityResourceCommand request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
