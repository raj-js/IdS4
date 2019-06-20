using IdS4.Application.Commands;
using IdS4.Application.Models.Paging;
using IdS4.Application.Models.Role;
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
    public class RoleController : ControllerBase
    {
        private readonly IRoleQueries _roleQueries;
        private readonly IMediator _mediator;

        public RoleController(
            IRoleQueries roleQueries,
            IMediator mediator)
        {
            _roleQueries = roleQueries;
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<Paged<VmRole>> Get([FromQuery]PagingQuery query)
        {
            return await _roleQueries.GetRoles(query);
        }

        [HttpGet("{id}")]
        public async Task<ApiResult> Get([FromRoute] string id)
        {
            if (string.IsNullOrEmpty(id)) return ApiResult.NotFound(id);

            var vmRole = await _roleQueries.GetRole(id);
            return vmRole == null ? ApiResult.NotFound(id) : ApiResult.Success(vmRole);
        }

        [HttpPost]
        public async Task<ApiResult> Add([FromBody]VmRole vm)
        {
            if (!ModelState.IsValid) return ApiResult.Failure(ModelState);

            var command = new AddRoleCommand(vm);
            var vmRole = await _mediator.Send(command);
            return vmRole == null ? ApiResult.Failure() : ApiResult.Success(vmRole);
        }

        [HttpPut]
        public async Task<ApiResult> Edit([FromBody] VmRole vm)
        {
            if (string.IsNullOrEmpty(vm.Id)) return ApiResult.NotFound(vm.Id);

            var command = new EditRoleCommand(vm);
            var vmRole = await _mediator.Send(command);
            return vmRole == null ? ApiResult.Failure() : ApiResult.Success(vmRole);
        }

        [HttpDelete("{ids}")]
        public async Task<ApiResult> Remove([FromRoute]string ids)
        {
            if (string.IsNullOrEmpty(ids)) return ApiResult.Failure();

            var command = new RemoveRoleCommand(ids);
            return await _mediator.Send(command) ? ApiResult.Success() : ApiResult.Failure();
        }
    }
}