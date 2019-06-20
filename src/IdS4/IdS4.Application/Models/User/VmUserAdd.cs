using Microsoft.AspNetCore.Identity;

namespace IdS4.Application.Models.User
{
    public class VmUserAdd
    {
        public IdentityResult Result { get; set; }

        public VmUser User { get; set; }

        public string DefaultPassword { get; set; }

        public static VmUserAdd Success(VmUser user, string defaultPassword)
        {
            return new VmUserAdd {User = user, DefaultPassword = defaultPassword, Result = IdentityResult.Success };
        }

        public static VmUserAdd Failure(IdentityResult result)
        {
            return new VmUserAdd {Result = result};
        }
    }
}
