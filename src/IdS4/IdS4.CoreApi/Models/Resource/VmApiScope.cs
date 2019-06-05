using System.Collections.Generic;

namespace IdS4.CoreApi.Models.Resource
{
    public class VmApiScope
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string DisplayName { get; set; }
        public string Description { get; set; }
        public bool Required { get; set; }
        public bool Emphasize { get; set; }
        public bool ShowInDiscoveryDocument { get; set; } = true;
        public List<VmApiScopeClaim> UserClaims { get; set; }

        public int ApiResourceId { get; set; }
    }
}
