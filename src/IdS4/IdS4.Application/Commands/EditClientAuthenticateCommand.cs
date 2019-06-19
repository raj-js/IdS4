using IdS4.Application.Models.Client;
using MediatR;
using System;

namespace IdS4.Application.Commands
{
    public class EditClientAuthenticateCommand: IRequest<VmClient.Authenticate>
    {
        public VmClient.Authenticate Authenticate { get; private set; }

        public EditClientAuthenticateCommand(VmClient.Authenticate authenticate)
        {
            Authenticate = authenticate ?? throw new ArgumentNullException(nameof(authenticate));
        }
    }
}
