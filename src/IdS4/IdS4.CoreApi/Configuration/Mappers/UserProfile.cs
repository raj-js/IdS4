using AutoMapper;
using IdS4.CoreApi.Models.User;
using IdS4.Identity;

namespace IdS4.CoreApi.Configuration.Mappers
{
    public class UserProfile: Profile
    {
        public UserProfile()
        {
            CreateMap<IdS4User, VmUser>()
                .ForMember(dst => dst.UserClaims, exp => exp.Ignore());
        }
    }
}
