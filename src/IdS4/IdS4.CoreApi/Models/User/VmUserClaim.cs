namespace IdS4.CoreApi.Models.User
{
    public class VmUserClaim
    {
        public int Id { get; set; }
        public string ClaimType { get; set; }
        public string ClaimValue { get; set; }

        public string UserId { get; set; }
    }
}
