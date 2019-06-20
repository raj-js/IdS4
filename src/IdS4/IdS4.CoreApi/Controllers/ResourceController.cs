using IdS4.Application.Commands;
using IdS4.Application.Models.Paging;
using IdS4.Application.Models.Resource;
using IdS4.Application.Queries;
using IdS4.CoreApi.Extensions;
using IdS4.CoreApi.Models.Results;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace IdS4.CoreApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [BearerAuthorize]
    public class ResourceController : ControllerBase
    {
        private readonly IResourceQueries _resourceQueries;
        private readonly IMediator _mediator;

        public ResourceController(IResourceQueries resourceQueries, IMediator mediator)
        {
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
            return id <= 0 ? ApiResult.NotFound(id) : ApiResult.Success(await _resourceQueries.GetApiResource(id));
        }

        [HttpGet("identity/{id}")]
        public async Task<ApiResult> GetIdentityResource([FromRoute] int id)
        {
            return id <= 0 ? ApiResult.NotFound(id) : ApiResult.Success(await _resourceQueries.GetIdentityResource(id));
        }

        [HttpPost("identity")]
        public async Task<ApiResult> AddIdentityResource([FromBody]VmIdentityResource vm)
        {
            if (vm.Id > 0) return ApiResult.NotFound(vm.Id);

            var command = new AddIdentityResourceCommand(vm);
            var vmResource = await _mediator.Send(command);
            return vmResource == null ? ApiResult.Failure() : ApiResult.Success(vmResource);
        }

        [HttpPut("identity")]
        public async Task<ApiResult> EditIdentityResource([FromBody]VmIdentityResource vm)
        {
            if (vm.Id <= 0) return ApiResult.NotFound(vm.Id);

            var command = new EditIdentityResourceCommand(vm);
            var vmResource = await _mediator.Send(command);
            return vmResource == null ? ApiResult.Failure() : ApiResult.Success(vmResource);
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
            var vmResource = await _mediator.Send(command);
            return vmResource == null ? ApiResult.Failure() : ApiResult.Success(vmResource);
        }

        [HttpPut("api")]
        public async Task<ApiResult> EditApiResource([FromBody]VmApiResource vm)
        {
            if (vm.Id <= 0) return ApiResult.NotFound(vm.Id);

            var command = new EditApiResourceCommand(vm);
            var vmResource = await _mediator.Send(command);
            return vmResource == null ? ApiResult.Failure() : ApiResult.Success(vmResource);
        }

        [HttpDelete("api/{resourceIds}")]
        public async Task<ApiResult> RemoveApiResource([FromRoute]string resourceIds)
        {
            var command = new RemoveApiResourceCommand(resourceIds);
            return await _mediator.Send(command) ? ApiResult.Success() : ApiResult.Failure();
        }
    }
}