using System.ComponentModel.DataAnnotations;

namespace IdS4.CoreApi.Models.Resource
{
    public class VmIdentityResourceProperty
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(32)]
        public string Key { get; set; }

        [Required]
        [MaxLength(256)]
        public string Value { get; set; }

        [Required]
        public int IdentityResourceId { get; set; }
    }
}
