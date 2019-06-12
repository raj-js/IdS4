using System.Collections.Generic;

namespace IdS4.CoreApi.Models.Role
{
    public class VmRole
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public List<VmRoleClaim> RoleClaims { get; set; }
    }
}
