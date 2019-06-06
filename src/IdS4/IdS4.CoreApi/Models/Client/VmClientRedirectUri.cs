namespace IdS4.CoreApi.Models.Client
{
    public class VmClientRedirectUri
    {
        public int Id { get; set; }
        public string RedirectUri { get; set; }

        public int ClientId { get; set; }
    }
}
