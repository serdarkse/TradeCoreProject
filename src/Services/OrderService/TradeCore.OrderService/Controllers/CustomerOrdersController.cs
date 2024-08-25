using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using TradeCore.OrderService.Filters;
using TradeCore.OrderService.Models.Request.Command.CustomerOrder;
using TradeCore.OrderService.Models.Request.Query.CustomerOrder;
using TradeCore.OrderService.Models.Response;
using TradeCore.OrderService.Models.Response.Command.CustomerOrder;
using TradeCore.OrderService.Models.Response.Query.CustomerOrder;

namespace TradeCore.OrderService.Controllers
{
    [BasicAuthFilter]
    [Authorize]
    [ApiController]
    [Produces("application/json")]
    [Route("api/[controller]")]
    public class CustomerOrdersController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<CustomerOrdersController> _logger;

        public CustomerOrdersController(IMediator mediator, ILogger<CustomerOrdersController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        [HttpGet("[action]")]
        [ProducesResponseType(200, Type = typeof(ResponseBase<GetCustomerOrderListQueryResponse>))]
        public async Task<IActionResult> GetCustomerOrderList([FromQuery] GetCustomerOrderListQueryRequest request, CancellationToken cancellationToken)
        {
            var response = await _mediator.Send(request, cancellationToken);

            _logger.LogInformation("GetCustomerOrderList İşlemi başarılı.");

            return Ok(response);
        }

        [HttpPost("[action]")]
        [ProducesResponseType(200, Type = typeof(ResponseBase<AddCustomerOrderCommandResponse>))]
        public async Task<IActionResult> AddCustomerOrder([FromBody] AddCustomerOrderCommandRequest request, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var response = await _mediator.Send(request, cancellationToken);
            _logger.LogInformation("CustomerOrder ekleme İşlemi başarılı.");

            return Ok(response);
        }

        [HttpPut("[action]")]
        [ProducesResponseType(200, Type = typeof(ResponseBase<UpdateCustomerOrderCommandResponse>))]
        public async Task<IActionResult> UpdateCustomerOrder([FromBody] UpdateCustomerOrderCommandRequest request, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var response = await _mediator.Send(request, cancellationToken);
            return Ok(response);
        }

        [HttpDelete("[action]")]
        [ProducesResponseType(200, Type = typeof(ResponseBase<DeleteCustomerOrderCommandResponse>))]
        public async Task<IActionResult> DeleteCustomerOrder([FromBody] DeleteCustomerOrderCommandRequest request, CancellationToken cancellationToken)
        {
            var response = await _mediator.Send(request, cancellationToken);
            return Ok(response);
        }
    }
}
