using AutoMapper;
using IdS4.Application.Models.Role;
using IdS4.Identity;

namespace IdS4.Application.Mappers
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
