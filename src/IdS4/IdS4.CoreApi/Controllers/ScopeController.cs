using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using IdS4.CoreApi.Extensions;
using IdS4.CoreApi.Models.Results;
using IdS4.CoreApi.Models.Scope;
using IdS4.DbContexts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace IdS4.CoreApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [BearerAuthorize]
    public class ScopeController: ControllerBase
    {
        private readonly IdS4ConfigurationDbContext _configurationDb;
        private readonly ILogger<ScopeController> _logger;
        private readonly IMapper _mapper;

        public ScopeController(IdS4ConfigurationDbContext configurationDb, ILogger<ScopeController> logger, IMapper mapper)
        {
            _configurationDb = configurationDb;
            _logger = logger;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ApiResult> Get()
        {
            var identityScope = await _configurationDb.IdentityResources
                .AsNoTracking()
                .Select(s => new VmSelectItem(s.DisplayName, s.Name))
                .ToListAsync();

            var apiScope = await _configurationDb.ApiScopes
                .AsNoTracking()
                .Select(s => new VmSelectItem(s.DisplayName, s.Name))
                .ToListAsync();

            return ApiResult.Success(identityScope.Concat(apiScope));
        }
    }
}
