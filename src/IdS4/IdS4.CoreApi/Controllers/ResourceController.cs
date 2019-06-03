using IdentityServer4.EntityFramework.Entities;
using IdS4.CoreApi.Extensions;
using IdS4.CoreApi.Models.Paging;
using IdS4.DbContexts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using AutoMapper;
using IdS4.CoreApi.Models.Resource;
using IdS4.CoreApi.Models.Results;

namespace IdS4.CoreApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [BearerAuthorize]
    public class ResourceController : ControllerBase
    {
        private readonly IdS4ConfigurationDbContext _configurationDb;
        private readonly ILogger<ResourceController> _logger;
        private readonly IMapper _mapper;

        public ResourceController(IdS4ConfigurationDbContext configurationDb, ILogger<ResourceController> logger, IMapper mapper)
        {
            _configurationDb = configurationDb;
            _logger = logger;
            _mapper = mapper;
        }

        [HttpGet("identity")]
        public async Task<Paged<IdentityResource>> GetIdentityResources([FromQuery]PageQuery query)
        {
            return Paged<IdentityResource>.From(
                await _configurationDb.IdentityResources.AsNoTracking()
                    .OrderBy(query.Sort ?? "Id")
                    .Skip(query.Skip)
                    .Take(query.Limit)
                    .ToListAsync(),
                await _configurationDb.IdentityResources.AsNoTracking().CountAsync()
                );
        }

        [HttpPost("identity/add")]
        public async Task<ApiResult> AddIdentityResource([FromBody]VmIdentityResource vm)
        {
            if (!ModelState.IsValid)
                return ModelState.ToApiResult();

            if (vm.Id > 0) return null;

            var resource = _mapper.Map<IdentityResource>(vm);

            var entry = await _configurationDb.IdentityResources.AddAsync(resource);
            await _configurationDb.SaveChangesAsync();

            return ApiResult.Success(entry.Entity);
        }
    }
}