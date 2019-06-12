namespace IdS4.CoreApi.Models.Role
{
    public class VmRoleClaim
    {
        public int Id { get; set; }
        public string ClaimType { get; set; }
        public string ClaimValue { get; set; }

        public string RoleId { get; set; }
    }
}
