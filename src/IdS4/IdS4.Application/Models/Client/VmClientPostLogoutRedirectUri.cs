namespace IdS4.Application.Models.Client
{
    public class VmClientPostLogoutRedirectUri
    {
        public int Id { get; set; }
        public string PostLogoutRedirectUri { get; set; }

        public int ClientId { get; set; }
    }
}
