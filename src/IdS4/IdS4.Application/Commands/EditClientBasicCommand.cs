using IdS4.Application.Models.Client;
using MediatR;
using System;

namespace IdS4.Application.Commands
{
    public class EditClientBasicCommandIRequest: IRequest<VmClient.Basic>
    {
        public VmClient.Basic Basic { get; private set; }

        public EditClientBasicCommandIRequest(VmClient.Basic basic)
        {
            Basic = basic ?? throw new ArgumentNullException(nameof(basic));
        }
    }
}
