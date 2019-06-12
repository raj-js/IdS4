using System.Linq;
using System.Threading.Tasks;
using IdS4.DbContexts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace IdS4.Management.Controllers
{
    [Route("api/[controller]")]
    public class SettingsController : ControllerBase
    {
        private readonly IdS4ConfigurationDbContext _configurationDb;
        private readonly IOptionsSnapshot<AppSettings> _settings;
        private readonly ILogger<SettingsController> _logger;

        public SettingsController(IOptionsSnapshot<AppSettings> settings, IdS4ConfigurationDbContext configurationDb, ILogger<SettingsController> logger)
        {
            _settings = settings;
            _configurationDb = configurationDb;
            _logger = logger;
        }

        [HttpGet]
        public AppSettings Default()
        {
            return _settings.Value;
        }

        [HttpGet("oidc")]
        public async Task<object> OidcSettings()
        {
            var clientId = _settings.Value.ClientId;
            var client = await _configurationDb.Clients.SingleOrDefaultAsync(s => s.ClientId.Equals(clientId));

            if (client == null)
            {
                _logger.LogError($"Client {clientId} not found!");
                return null;
            }

            return new
            {
                stsServer = "https://localhost:5001"
            };
        }
    }
}
