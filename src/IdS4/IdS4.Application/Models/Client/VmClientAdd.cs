using System.ComponentModel.DataAnnotations;

namespace IdS4.Application.Models.Client
{
    public class VmClientAdd
    {
        [Required]
        [MaxLength(32)]
        public string ClientId { get; set; }

        [Required]
        [MaxLength(32)]
        public string ClientName { get; set; }

        public VmClientType Type { get; set; }
    }
}
