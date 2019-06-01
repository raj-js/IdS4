using System.Collections.Generic;
using System.Threading.Tasks;
using IdentityServer4.EntityFramework.Entities;
using IdS4.CoreApi.Extensions;
using IdS4.DbContexts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace IdS4.CoreApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [BearerAuthorize]
    public class ResourceController : ControllerBase
    {
        private readonly IdS4ConfigurationDbContext _configurationDb;
        private readonly ILogger<ResourceController> _logger;

        public ResourceController(IdS4ConfigurationDbContext configurationDb, ILogger<ResourceController> logger)
        {
            _configurationDb = configurationDb;
            _logger = logger;
        }

        [HttpGet("identity")]
        public async Task<List<IdentityResource>> GetIdentityResources()
        {
            return await _configurationDb.IdentityResources
                .AsNoTracking()
                .ToListAsync();
        }
    }
}