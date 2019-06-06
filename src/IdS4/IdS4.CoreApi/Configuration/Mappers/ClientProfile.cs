using AutoMapper;
using IdentityServer4.EntityFramework.Entities;
using IdS4.CoreApi.Models.Client;

namespace IdS4.CoreApi.Configuration.Mappers
{
    public class ClientProfile: Profile
    {
        public ClientProfile()
        {
            CreateMap<VmClientClaim, ClientClaim>()
                .ForMember(dst => dst.Client, exp => exp.Ignore());

            CreateMap<VmClientCorsOrigin, ClientCorsOrigin>()
                .ForMember(dst => dst.Client, exp => exp.Ignore());

            CreateMap<VmClientGrantType, ClientGrantType>()
                .ForMember(dst => dst.Client, exp => exp.Ignore());

            CreateMap<VmClientIdPRestriction, ClientIdPRestriction>()
                .ForMember(dst => dst.Client, exp => exp.Ignore());

            CreateMap<VmClientPostLogoutRedirectUri, ClientPostLogoutRedirectUri>()
                .ForMember(dst => dst.Client, exp => exp.Ignore());

            CreateMap<VmClientProperty, ClientProperty>()
                .ForMember(dst => dst.Client, exp => exp.Ignore());

            CreateMap<VmClientRedirectUri, ClientRedirectUri>()
                .ForMember(dst => dst.Client, exp => exp.Ignore());

            CreateMap<VmClientScope, ClientScope>()
                .ForMember(dst => dst.Client, exp => exp.Ignore());

            CreateMap<VmClientSecret, ClientSecret>()
                .ForMember(dst => dst.Client, exp => exp.Ignore());
        }
    }
}
