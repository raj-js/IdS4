using IdS4.Application.Queries;
using IdS4.CoreApi.Extensions;
using IdS4.CoreApi.Models.Results;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace IdS4.CoreApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [BearerAuthorize]
    public class ScopeController: ControllerBase
    {
        private readonly IScopeQueries _scopeQueries;

        public ScopeController(IScopeQueries scopeQueries)
        {
            _scopeQueries = scopeQueries;
        }

        [HttpGet]
        public async Task<ApiResult> Get()
        {
            return ApiResult.Success(await _scopeQueries.GetClientScopeSelectItems());
        }
    }
}
