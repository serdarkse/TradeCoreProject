using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TradeCore.OrderService.Filters;
using TradeCore.OrderService.Models.Request.Command.Product;
using TradeCore.OrderService.Models.Request.Query.Product;
using TradeCore.OrderService.Models.Response;
using TradeCore.OrderService.Models.Response.Command.Product;
using TradeCore.OrderService.Models.Response.Query.Product;

namespace TradeCore.OrderService.Controllers
{
    [BasicAuthFilter]
    [Authorize]
    [ApiController]
    [Produces("application/json")]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<ProductsController> _logger;

        public ProductsController(IMediator mediator, ILogger<ProductsController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        [HttpGet("[action]")]
        [ProducesResponseType(200, Type = typeof(ResponseBase<GetProductListQueryResponse>))]
        public async Task<IActionResult> GetProductList([FromQuery] GetProductListQueryRequest request, CancellationToken cancellationToken)
        {
            var response = await _mediator.Send(request, cancellationToken);
            _logger.LogInformation("Product List çekme işlemi başarılı.");

            return Ok(response);
        }

        [HttpPost("[action]")]
        [ProducesResponseType(200, Type = typeof(ResponseBase<AddProductCommandResponse>))]
        public async Task<IActionResult> AddProduct([FromBody] AddProductCommandRequest request, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var response = await _mediator.Send(request, cancellationToken);
            _logger.LogInformation("Product ekleme işlemi başarılı.");

            return Ok(response);
        }

        [HttpPut("[action]")]
        [ProducesResponseType(200, Type = typeof(ResponseBase<UpdateProductCommandResponse>))]
        public async Task<IActionResult> UpdateProduct([FromBody] UpdateProductCommandRequest request, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var response = await _mediator.Send(request, cancellationToken);
            _logger.LogInformation("Product güncelleme işlemi başarılı.");

            return Ok(response);
        }

        [HttpDelete("[action]")]
        [ProducesResponseType(200, Type = typeof(ResponseBase<DeleteProductCommandResponse>))]
        public async Task<IActionResult> DeleteProduct([FromBody] DeleteProductCommandRequest request, CancellationToken cancellationToken)
        {
            var response = await _mediator.Send(request, cancellationToken);
            _logger.LogInformation("Product silme işlemi başarılı.");

            return Ok(response);
        }
    }
}
