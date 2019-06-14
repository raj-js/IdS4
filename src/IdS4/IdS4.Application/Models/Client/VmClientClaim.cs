namespace IdS4.Application.Models.Client
{
    public class VmClientClaim
    {
        public int Id { get; set; }
        public string Type { get; set; }
        public string Value { get; set; }

        public int ClientId { get; set; }
    }
}
