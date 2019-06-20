using IdS4.Application.Commands;
using IdS4.Application.Models.Client;
using IdS4.Application.Models.Paging;
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
    public class ClientController : ControllerBase
    {
        private readonly IClientQueries _clientQueries;
        private readonly IMediator _mediator;

        public ClientController(IClientQueries clientQueries, IMediator mediator)
        {
            _clientQueries = clientQueries;
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<Paged<VmClient>> Get([FromQuery]PagingQuery query)
        {
            return await _clientQueries.GetClients(query);
        }

        [HttpGet("{id}")]
        public async Task<ApiResult> Get([FromRoute]int id)
        {
            if (id <= 0) return ApiResult.NotFound(id);

            var vmSplitClient = await _clientQueries.GetClient(id);
            return vmSplitClient == null ? ApiResult.NotFound(id) : ApiResult.Success(vmSplitClient);
        }

        [HttpPost]
        public async Task<ApiResult> Add([FromBody]VmClientAdd vm)
        {
            if (!ModelState.IsValid) return ApiResult.Failure(ModelState);

            var command = new AddClientCommand(vm);
            var vmClient = await _mediator.Send(command);
            return vmClient == null ? ApiResult.Failure() : ApiResult.Success(vmClient);
        }

        [HttpPatch("basic")]
        public async Task<ApiResult> Edit([FromBody]VmClient.Basic vm)
        {
            if (vm.Id <= 0) return ApiResult.NotFound(vm.Id);

            var command = new EditClientBasicCommand(vm);
            var vmClient = await _mediator.Send(command);
            return vmClient == null ? ApiResult.Failure() : ApiResult.Success(vmClient);
        }

        [HttpPatch("authenticate")]
        public async Task<ApiResult> Edit([FromBody]VmClient.Authenticate vm)
        {
            if (vm.Id <= 0) return ApiResult.NotFound(vm.Id);

            var command = new EditClientAuthenticateCommand(vm);
            var vmClient = await _mediator.Send(command);
            return vmClient == null ? ApiResult.Failure() : ApiResult.Success(vmClient);
        }

        [HttpPatch("token")]
        public async Task<ApiResult> Edit([FromBody]VmClient.Token vm)
        {
            if (vm.Id <= 0) return ApiResult.NotFound(vm.Id);

            var command = new EditClientTokenCommand(vm);
            var vmClient = await _mediator.Send(command);
            return vmClient == null ? ApiResult.Failure() : ApiResult.Success(vmClient);
        }

        [HttpPatch("consent")]
        public async Task<ApiResult> Edit([FromBody]VmClient.Consent vm)
        {
            if (vm.Id <= 0) return ApiResult.NotFound(vm.Id);

            var command = new EditClientConsentCommand(vm);
            var vmClient = await _mediator.Send(command);
            return vmClient == null ? ApiResult.Failure() : ApiResult.Success(vmClient);
        }

        [HttpPatch("device")]
        public async Task<ApiResult> Edit([FromBody]VmClient.Device vm)
        {
            if (vm.Id <= 0) return ApiResult.NotFound(vm.Id);

            var command = new EditClientDeviceCommand(vm);
            var vmClient = await _mediator.Send(command);
            return vmClient == null ? ApiResult.Failure() : ApiResult.Success(vmClient);
        }

        [HttpDelete("{ids}")]
        public async Task<ApiResult> Remove([FromRoute]string ids)
        {
            if (string.IsNullOrEmpty(ids)) return ApiResult.Failure();

            var command = new RemoveClientCommand(ids);
            return await _mediator.Send(command) ? ApiResult.Success() : ApiResult.Failure();
        }
    }
}