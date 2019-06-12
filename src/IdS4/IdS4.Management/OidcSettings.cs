using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace IdS4.Management
{
    public class OidcSettings
    {
        [JsonProperty("stsServer")]
        public string StsServer { get; set; }

        [JsonProperty("redirect_url")]
        public string RedirectUrl { get; set; }

        [JsonProperty("client_id")]
        public string ClientId { get; set; }

        [JsonProperty("response_type")]
        public string ResponseType { get; set; }

        [JsonProperty("scope")]
        public string Scope { get; set; }

        [JsonProperty("post_logout_redirect_uri")]
        public string PostLogoutRedirectUri { get; set; }

        [JsonProperty("start_checksession")]
        public string StartCheckSession { get; set; }

        [JsonProperty("silent_renew")]
        public string SilentRenew { get; set; }

        [JsonProperty("silent_renew_url")]
        public string SilentRenewUrl { get; set; }

        [JsonProperty("post_login_route")]
        public string PostLoginRoute { get; set; }

        [JsonProperty("forbidden_route")]
        public string ForbiddenRoute { get; set; }

        [JsonProperty("unauthorized_route")]
        public string UnauthorizedRoute { get; set; }

        [JsonProperty("log_console_warning_active")]
        public string LogConsoleWarningActive { get; set; }

        [JsonProperty("log_console_debug_active")]
        public string LogConsoleDebugActive { get; set; }

        [JsonProperty("max_id_token_iat_offset_allowed_in_seconds")]
        public string MaxIdTokenIatOffsetAllowedInSeconds { get; set; }
    }
}
