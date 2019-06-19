using AutoMapper;
using IdS4.Application.Commands;
using IdS4.Application.Models.Paging;
using IdS4.Application.Models.Resource;
using IdS4.Application.Queries;
using IdS4.CoreApi.Extensions;
using IdS4.CoreApi.Models.Results;
using IdS4.DbContexts;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

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
        private readonly IResourceQueries _resourceQueries;
        private readonly IMediator _mediator;

        public ResourceController(IdS4ConfigurationDbContext configurationDb, ILogger<ResourceController> logger, IMapper mapper, IResourceQueries resourceQueries, IMediator mediator)
        {
            _configurationDb = configurationDb;
            _logger = logger;
            _mapper = mapper;
            _resourceQueries = resourceQueries;
            _mediator = mediator;
        }

        [HttpGet("identity")]
        public async Task<Paged<VmIdentityResource>> GetIdentityResources([FromQuery]PagingQuery query)
        {
            return await _resourceQueries.GetIdentityResources(query);
        }

        [HttpGet("api")]
        public async Task<Paged<VmApiResource>> GetApiResources([FromQuery] PagingQuery query)
        {
            return await _resourceQueries.GetApiResources(query);
        }

        [HttpGet("api/{id}")]
        public async Task<ApiResult> GetApiResource([FromRoute] int id)
        {
            if (id <= 0) return ApiResult.NotFound(id);

            return ApiResult.Success(await _resourceQueries.GetApiResource(id));
        }

        [HttpGet("identity/{id}")]
        public async Task<ApiResult> GetIdentityResource([FromRoute] int id)
        {
            if (id <= 0) return ApiResult.NotFound(id);

            return ApiResult.Success(await _resourceQueries.GetIdentityResource(id));
        }

        [HttpPost("identity")]
        public async Task<ApiResult> AddIdentityResource([FromBody]VmIdentityResource vm)
        {
            if (vm.Id > 0) return ApiResult.NotFound(vm.Id);

            var command = new AddIdentityResourceCommand(vm);
            return ApiResult.Success(await _mediator.Send(command));
        }

        [HttpPut("identity")]
        public async Task<ApiResult> EditIdentityResource([FromBody]VmIdentityResource vm)
        {
            if (vm.Id <= 0) return ApiResult.NotFound(vm.Id);

            var command = new EditIdentityResourceCommand(vm);
            return ApiResult.Success(await _mediator.Send(command));
        }

        [HttpDelete("identity/{resourceIds}")]
        public async Task<ApiResult> RemoveIdentityResource([FromRoute] string resourceIds)
        {
            if (string.IsNullOrEmpty(resourceIds)) return ApiResult.Failure();

            var command = new RemoveIdentityResourceCommand(resourceIds);
            return await _mediator.Send(command) ? ApiResult.Success() : ApiResult.Failure();
        }

        [HttpPost("api")]
        public async Task<ApiResult> AddApiResource([FromBody]VmApiResource vm)
        {
            if (vm.Id > 0) return ApiResult.NotFound(vm.Id);

            var command = new AddApiResourceCommand(vm);
            return ApiResult.Success(await _mediator.Send(command));
        }

        [HttpPut("api")]
        public async Task<ApiResult> EditApiResource([FromBody]VmApiResource vm)
        {
            if (vm.Id <= 0) return ApiResult.NotFound(vm.Id);

            var command = new EditApiResourceCommand(vm);
            var vmApiResource = await _mediator.Send(command);
            if (vmApiResource == null)
                return ApiResult.NotFound(vm.Id);

            return ApiResult.Success(vmApiResource);
        }

        [HttpDelete("api/{resourceIds}")]
        public async Task<ApiResult> RemoveApiResource([FromRoute]string resourceIds)
        {
            var command = new RemoveApiResourceCommand(resourceIds);
            return await _mediator.Send(command) ? ApiResult.Success() : ApiResult.Failure();
        }
    }
}