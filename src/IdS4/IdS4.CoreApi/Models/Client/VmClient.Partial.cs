using AutoMapper;
using System.Collections.Generic;

namespace IdS4.CoreApi.Models.Client
{
    public partial class VmClient
    {
        public class Basic
        {
            public int Id { get; set; }
            public string ClientId { get; set; }
            public string ClientName { get; set; }
            public bool Enabled { get; set; } = true;
            public string ProtocolType { get; set; } = "oidc";
            public bool RequireClientSecret { get; set; } = true;
            public List<VmClientSecret> ClientSecrets { get; set; } = new List<VmClientSecret>();
            public List<VmClientGrantType> AllowedGrantTypes { get; set; } = new List<VmClientGrantType>();
            public bool RequirePkce { get; set; }
            public bool AllowPlainTextPkce { get; set; }
            public List<VmClientRedirectUri> RedirectUris { get; set; } = new List<VmClientRedirectUri>();
            public List<VmClientScope> AllowedScopes { get; set; } = new List<VmClientScope>();
            public bool AllowOfflineAccess { get; set; }
            public bool AllowAccessTokensViaBrowser { get; set; }
            public List<VmClientProperty> Properties { get; set; } = new List<VmClientProperty>();
            public string Description { get; set; }

            public void ApplyChangeToClient(VmClient vm)
            {
                vm.ClientId = ClientId;
                vm.ClientName = ClientName;
                vm.Enabled = Enabled;
                vm.ProtocolType = ProtocolType;
                vm.RequireClientSecret = RequireClientSecret;
                vm.ClientSecrets = ClientSecrets;
                vm.AllowedGrantTypes = AllowedGrantTypes;
                vm.RequirePkce = RequirePkce;
                vm.AllowPlainTextPkce = AllowPlainTextPkce;
                vm.RedirectUris = RedirectUris;
                vm.AllowedScopes = AllowedScopes;
                vm.AllowOfflineAccess = AllowOfflineAccess;
                vm.AllowAccessTokensViaBrowser = AllowAccessTokensViaBrowser;
                vm.Properties = Properties;
                vm.Description = Description;
            }
        }

        public Basic ToBasic(IMapper mapper)
        {
            return mapper.Map<Basic>(this);
        }

        public class Authenticate
        {
            public int Id { get; set; }
            public List<VmClientPostLogoutRedirectUri> PostLogoutRedirectUris { get; set; } = new List<VmClientPostLogoutRedirectUri>();
            public string FrontChannelLogoutUri { get; set; }
            public bool FrontChannelLogoutSessionRequired { get; set; } = true;
            public string BackChannelLogoutUri { get; set; }
            public bool BackChannelLogoutSessionRequired { get; set; } = true;
            public bool EnableLocalLogin { get; set; } = true;
            public List<VmClientIdPRestriction> IdentityProviderRestrictions { get; set; } = new List<VmClientIdPRestriction>();
            public int? UserSsoLifetime { get; set; }

            public void ApplyChangeToClient(VmClient vm)
            {
                vm.PostLogoutRedirectUris = PostLogoutRedirectUris ?? new List<VmClientPostLogoutRedirectUri>();
                vm.FrontChannelLogoutUri = FrontChannelLogoutUri;
                vm.FrontChannelLogoutSessionRequired = FrontChannelLogoutSessionRequired;
                vm.BackChannelLogoutUri = BackChannelLogoutUri;
                vm.BackChannelLogoutSessionRequired = BackChannelLogoutSessionRequired;
                vm.EnableLocalLogin = EnableLocalLogin;
                vm.IdentityProviderRestrictions = IdentityProviderRestrictions ?? new List<VmClientIdPRestriction>();
                vm.UserSsoLifetime = UserSsoLifetime;
            }
        }

        public Authenticate ToAuthenticate(IMapper mapper)
        {
            return mapper.Map<Authenticate>(this);
        }

        public class Token
        {
            public int Id { get; set; }
            public int IdentityTokenLifetime { get; set; } = 300;
            public int AccessTokenLifetime { get; set; } = 3600;
            public int AuthorizationCodeLifetime { get; set; } = 300;
            public int AbsoluteRefreshTokenLifetime { get; set; } = 2592000;
            public int SlidingRefreshTokenLifetime { get; set; } = 1296000;
            public int RefreshTokenUsage { get; set; } = 1;
            public int RefreshTokenExpiration { get; set; } = 1;
            public bool UpdateAccessTokenClaimsOnRefresh { get; set; }
            public int AccessTokenType { get; set; }
            public bool IncludeJwtId { get; set; }
            public List<VmClientCorsOrigin> AllowedCorsOrigins { get; set; } = new List<VmClientCorsOrigin>();
            public List<VmClientClaim> Claims { get; set; } = new List<VmClientClaim>();
            public bool AlwaysSendClientClaims { get; set; }
            public bool AlwaysIncludeUserClaimsInIdToken { get; set; }
            public string ClientClaimsPrefix { get; set; } = "client_";
            public string PairWiseSubjectSalt { get; set; }

            public void ApplyChangeToClient(VmClient vm)
            {
                vm.IdentityTokenLifetime = IdentityTokenLifetime;
                vm.AccessTokenLifetime = AccessTokenLifetime;
                vm.AuthorizationCodeLifetime = AuthorizationCodeLifetime;
                vm.AbsoluteRefreshTokenLifetime = AbsoluteRefreshTokenLifetime;
                vm.SlidingRefreshTokenLifetime = SlidingRefreshTokenLifetime;
                vm.RefreshTokenUsage = RefreshTokenUsage;
                vm.RefreshTokenExpiration = RefreshTokenExpiration;
                vm.UpdateAccessTokenClaimsOnRefresh = UpdateAccessTokenClaimsOnRefresh;
                vm.AccessTokenType = AccessTokenType;
                vm.IncludeJwtId = IncludeJwtId;
                vm.AllowedCorsOrigins = AllowedCorsOrigins ?? new List<VmClientCorsOrigin>();
                vm.Claims = Claims ?? new List<VmClientClaim>();
                vm.AlwaysSendClientClaims = AlwaysSendClientClaims;
                vm.AlwaysIncludeUserClaimsInIdToken = AlwaysIncludeUserClaimsInIdToken;
                vm.ClientClaimsPrefix = ClientClaimsPrefix;
                vm.PairWiseSubjectSalt = PairWiseSubjectSalt;
            }
        }

        public Token ToToken(IMapper mapper)
        {
            return mapper.Map<Token>(this);
        }

        public class Consent
        {
            public int Id { get; set; }
            public bool RequireConsent { get; set; } = true;
            public bool AllowRememberConsent { get; set; } = true;
            public int? ConsentLifetime { get; set; }
            public string ClientUri { get; set; }
            public string LogoUri { get; set; }

            public void ApplyChangeToClient(VmClient vm)
            {
                vm.RequireConsent = RequireConsent;
                vm.AllowRememberConsent = AllowRememberConsent;
                vm.ConsentLifetime = ConsentLifetime;
                vm.ClientUri = ClientUri;
                vm.LogoUri = LogoUri;
            }
        }

        public Consent ToConsent(IMapper mapper)
        {
            return mapper.Map<Consent>(this);
        }

        public class Device
        {
            public int Id { get; set; }
            public string UserCodeType { get; set; }
            public int DeviceCodeLifetime { get; set; } = 300;

            public void ApplyChangeToClient(VmClient vm)
            {
                vm.UserCodeType = UserCodeType;
                vm.DeviceCodeLifetime = DeviceCodeLifetime;
            }
        }

        public Device ToDevice(IMapper mapper)
        {
            return mapper.Map<Device>(this);
        }
    }
}
