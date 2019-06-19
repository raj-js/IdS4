using System;
using IdS4.Application.Models.Client;
using MediatR;

namespace IdS4.Application.Commands
{
    public class AddClientCommand : IRequest<VmClient>
    {
        public VmClientAdd Client { get; private set; }

        public AddClientCommand(VmClientAdd client)
        {
            Client = client ?? throw new ArgumentNullException(nameof(client));
        }
    }
}
