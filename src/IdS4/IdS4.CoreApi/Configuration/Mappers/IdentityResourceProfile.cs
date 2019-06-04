using AutoMapper;
using IdentityServer4.EntityFramework.Entities;
using IdS4.CoreApi.Models.Resource;

namespace IdS4.CoreApi.Configuration.Mappers
{
    public class IdentityResourceProfile : Profile
    {
        public IdentityResourceProfile()
        {
            CreateMap<VmIdentityClaim, IdentityClaim>()
                .ForMember(dst => dst.IdentityResource, exp => exp.Ignore());
        }
    }
}
