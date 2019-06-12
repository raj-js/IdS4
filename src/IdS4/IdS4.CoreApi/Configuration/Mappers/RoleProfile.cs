using AutoMapper;
using IdS4.CoreApi.Models.Role;
using IdS4.Identity;

namespace IdS4.CoreApi.Configuration.Mappers
{
    public class RoleProfile : Profile
    {
        public RoleProfile()
        {
            CreateMap<VmRole, IdS4Role>()
                .ForMember(dst => dst.NormalizedName, exp => exp.Ignore());

            CreateMap<VmRoleClaim, IdS4RoleClaim>()
                .ForMember(dst => dst.Role, exp => exp.Ignore());
        }
    }
}
