using IdS4.Application.Models.Client;
using MediatR;
using System;

namespace IdS4.Application.Commands
{
    public class EditClientConsentCommand : IRequest<VmClient.Consent>
    {
        public VmClient.Consent Consent { get; private set; }

        public EditClientConsentCommand(VmClient.Consent consent)
        {
            Consent = consent ?? throw new ArgumentNullException(nameof(consent));
        }
    }
}
