﻿using AutoMapper;
using IdentityServer4.EntityFramework.Entities;
using IdS4.Application.Models.Resource;

namespace IdS4.Application.Mappers
{
    public class ResourceProfile : Profile
    {
        public ResourceProfile()
        {
            CreateMap<VmIdentityClaim, IdentityClaim>()
                .ForMember(dst => dst.IdentityResource, exp => exp.Ignore());

            CreateMap<VmIdentityResourceProperty, IdentityResourceProperty>()
                .ForMember(dst => dst.IdentityResource, exp => exp.Ignore());

            CreateMap<VmApiScope, ApiScope>()
                .ForMember(dst => dst.ApiResource, exp => exp.Ignore());

            CreateMap<VmApiSecret, ApiSecret>()
                .ForMember(dst => dst.ApiResource, exp => exp.Ignore());

            CreateMap<VmApiResourceClaim, ApiResourceClaim>()
                .ForMember(dst => dst.ApiResource, exp => exp.Ignore());

            CreateMap<VmApiResourceProperty, ApiResourceProperty>()
                .ForMember(dst => dst.ApiResource, exp => exp.Ignore());

            CreateMap<VmApiScopeClaim, ApiScopeClaim>()
                .ForMember(dst => dst.ApiScope, exp => exp.Ignore());
        }
    }
}
