﻿using System.Collections.Generic;

namespace IdS4.Application.Models.Role
{
    public class VmRole
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string ConcurrencyStamp { get; set; }
        public List<VmRoleClaim> RoleClaims { get; set; }
    }
}
