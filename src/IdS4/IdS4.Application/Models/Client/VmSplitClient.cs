using AutoMapper;

namespace IdS4.Application.Models.Client
{
    public class VmSplitClient
    {
        public VmClient.Basic Basic { get; set; }
        public VmClient.Authenticate Authenticate { get; set; }
        public VmClient.Token Token { get; set; }
        public VmClient.Consent Consent { get; set; }
        public VmClient.Device Device { get; set; }

        public VmSplitClient()
        {
            
        }

        public VmSplitClient(IMapper mapper, VmClient client)
        {
            Basic = client.ToBasic(mapper);
            Authenticate = client.ToAuthenticate(mapper);
            Token = client.ToToken(mapper);
            Consent = client.ToConsent(mapper);
            Device = client.ToDevice(mapper);
        }
    }
}
