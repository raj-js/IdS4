using System;

namespace IdS4.CoreApi.Models.Client
{
    public class VmClientSecret
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public string Value { get; set; }
        public DateTime? Expiration { get; set; }
        public string Type { get; set; } = "SharedSecret";
        public DateTime Created { get; set; } = DateTime.UtcNow;
        public int ClientId { get; set; }
    }
}
