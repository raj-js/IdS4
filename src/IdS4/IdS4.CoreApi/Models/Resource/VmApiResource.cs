using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdS4.CoreApi.Models.Resource
{
    public class VmApiResource
    {
        public int Id { get; set; }
        public bool Enabled { get; set; } = true;
        public string Name { get; set; }
        public string DisplayName { get; set; }
        public string Description { get; set; }
        public List<VmApiSecret> Secrets { get; set; }
        public List<VmApiScope> Scopes { get; set; }
        public List<VmApiResourceClaim> UserClaims { get; set; }
        public List<VmApiResourceProperty> Properties { get; set; }
        public DateTime Created { get; set; } = DateTime.UtcNow;
        public DateTime? Updated { get; set; }
        public DateTime? LastAccessed { get; set; }
        public bool NonEditable { get; set; }
    }
}
