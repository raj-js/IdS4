using IdS4.Application.Models.Client;
using MediatR;
using System;

namespace IdS4.Application.Commands
{
    public class EditClientDeviceCommand : IRequest<VmClient.Device>
    {
        public VmClient.Device Device { get; private set; }

        public EditClientDeviceCommand(VmClient.Device device)
        {
            Device = device ?? throw new ArgumentNullException(nameof(device));
        }
    }
}
