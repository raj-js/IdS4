using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace IdS4.Management.Controllers
{
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
    }
}
