using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace IdS4.Management.Controllers
{
    [AllowAnonymous]
    [Route("api/[controller]")]
    public class SettingsController : ControllerBase
    {
        private readonly IOptionsSnapshot<AppSettings> _settings;

        public SettingsController(IOptionsSnapshot<AppSettings> settings)
        {
            _settings = settings;
        }

        [HttpGet]
        public AppSettings Default()
        {
            return _settings.Value;
        }

        /// <summary>
        /// ªÒ»° spa oidc client ≈‰÷√
        /// </summary>
        /// <returns></returns>
        [HttpGet("Oidc")]
        public object Oidc()
        {
            return new
            {
                stsServer = "https://localhost:5001",
                redirect_url = "https://localhost:5002/#/callback",
                client_id = "IdS4.Management.Spa",
                response_type = "code",
                scope = "openid profile email coreApi.full_access",
                post_logout_redirect_uri = "/social-login",
                start_checksession = true,
                silent_renew = true,
                silent_renew_url = "/silent-renew",
                post_login_route = "/home",
                forbidden_route = "/forbidden",
                unauthorized_route = "/unauthorized",
                log_console_warning_active = false,
                log_console_debug_active = false,
                max_id_token_iat_offset_allowed_in_seconds = 10
            };
        }
    }
}
