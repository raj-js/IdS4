using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using IdentityServer4.EntityFramework.Entities;

namespace IdS4.CoreApi.Models.Resource
{
    public class VmIdentityResource
    {
        public int Id { get; set; }
        public bool Enabled { get; set; } = true;

        [Required]
        [MaxLength(32)]
        public string Name { get; set; }

        [Required]
        [MaxLength(32)]
        public string DisplayName { get; set; }

        public string Description { get; set; }
        public bool Required { get; set; }
        public bool Emphasize { get; set; }
        public bool ShowInDiscoveryDocument { get; set; } = true;
        public List<VmIdentityClaim> UserClaims { get; set; }
        public List<VmIdentityResourceProperty> Properties { get; set; }
        public DateTime Created { get; set; } = DateTime.UtcNow;
        public DateTime? Updated { get; set; }
        public bool NonEditable { get; set; }
    }
}
