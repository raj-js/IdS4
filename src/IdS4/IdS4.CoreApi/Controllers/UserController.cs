using IdS4.Application.Commands;
using IdS4.Application.Models.Paging;
using IdS4.Application.Models.User;
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
    public class UserController : ControllerBase
    {
        private readonly IUserQueries _userQueries;
        private readonly IMediator _mediator;

        public UserController(
            IUserQueries userQueries, 
            IMediator mediator)
        {
            _userQueries = userQueries;
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<Paged<VmUser>> Get([FromQuery]PagingQuery query)
        {
            return await _userQueries.GetUsers(query);
        }

        [HttpGet("{id}")]
        public async Task<ApiResult> Get([FromRoute] string id)
        {
            if (string.IsNullOrEmpty(id)) return ApiResult.NotFound(id);

            var vmUser = await _userQueries.GetUser(id);
            return vmUser == null ? ApiResult.NotFound(id) : ApiResult.Success(vmUser);
        }

        [HttpPost]
        public async Task<ApiResult> Add([FromBody] VmUser vm)
        {
            var command = new AddUserCommand(vm);
            var vmUserAdd = await _mediator.Send(command);

            return vmUserAdd.Result?.Succeeded == true ? 
                ApiResult.Success(vmUserAdd.User, msg: $"默认密码为：{vmUserAdd.DefaultPassword}") : 
                ApiResult.Failure(vmUserAdd.Result?.Errors);
        }

        [HttpPut]
        public async Task<ApiResult> Edit([FromBody] VmUser vm)
        {
            if (string.IsNullOrEmpty(vm.Id)) return ApiResult.NotFound(vm.Id);

            var command = new EditUserCommand(vm);
            var vmUser = await _mediator.Send(command);
            return vmUser == null ? ApiResult.Failure() : ApiResult.Success(vm);
        }

        [HttpDelete("{ids}")]
        public async Task<ApiResult> Remove([FromRoute]string ids)
        {
            if (string.IsNullOrEmpty(ids)) return ApiResult.Failure();

            var command = new RemoveUserCommand(ids);
            return await _mediator.Send(command) ? ApiResult.Success() : ApiResult.Failure();
        }
    }
}
