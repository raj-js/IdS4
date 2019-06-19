using IdS4.Application.Models.Client;
using MediatR;
using System;

namespace IdS4.Application.Commands
{
    public class EditClientTokenCommand : IRequest<VmClient.Token>
    {
        public VmClient.Token Token { get; private set; }

        public EditClientTokenCommand(VmClient.Token token)
        {
            Token = token ?? throw new ArgumentNullException(nameof(token));
        }
    }
}
