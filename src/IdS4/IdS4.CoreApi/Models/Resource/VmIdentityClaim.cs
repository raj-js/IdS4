using System.ComponentModel.DataAnnotations;

namespace IdS4.CoreApi.Models.Resource
{
    public class VmIdentityClaim
    {
        public int Id { get; set; }

        [Required]
        public string Type { get; set; }

        [Required]
        public int IdentityResourceId { get; set; }
    }
}
