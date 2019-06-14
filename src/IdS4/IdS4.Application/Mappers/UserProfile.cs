using AutoMapper;
using IdS4.CoreApi.Models.User;
using IdS4.Identity;

namespace IdS4.Application.Mappers
{
    public class UserProfile: Profile
    {
        public UserProfile()
        {
            CreateMap<VmUser, IdS4User>()
                .ForMember(dst => dst.UserRoles, exp => exp.Ignore())
                .ForMember(dst => dst.NormalizedUserName, exp => exp.Ignore())
                .ForMember(dst => dst.NormalizedEmail, exp => exp.Ignore())
                .ForMember(dst => dst.PasswordHash, exp => exp.Ignore())
                .ForMember(dst => dst.SecurityStamp, exp => exp.Ignore())
                .ForMember(dst => dst.PhoneNumberConfirmed, exp => exp.Ignore());

            CreateMap<VmUserClaim, IdS4UserClaim>()
                .ForMember(dst => dst.User, exp => exp.Ignore());

            CreateMap<VmUserRole, IdS4UserRole>()
                .ForMember(dst => dst.User, exp => exp.Ignore())
                .ForMember(dst => dst.Role, exp => exp.Ignore());
        }
    }
}
