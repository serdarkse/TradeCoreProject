using MediatR;
using Microsoft.AspNetCore.Mvc;
using TradeCore.AuthService.Filters;
using TradeCore.AuthService.Models.Request.Command;
using TradeCore.AuthService.Models.Request.Query;
using TradeCore.AuthService.Models.Response;
using TradeCore.AuthService.Models.Response.Command;
using TradeCore.AuthService.Models.Response.Query;

namespace TradeCore.AuthService.Controllers
{
    [BasicAuthFilter]
    [ApiController]
    [Produces("application/json", "text/plain")]
    [Route("api/[controller]")]
    public class AppOperationClaimsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AppOperationClaimsController(IMediator mediator)
        {
            _mediator = mediator;
        }
        ///<summary>
        ///List OperationClaims 
        ///</summary>
        ///<remarks>bla bla bla OperationClaims</remarks>
        ///<return>OperationClaims List</return>
        ///<response code="200"></response>  
        [Produces("application/json", "text/plain")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResponseBase<GetAppOperationClaimsQueryResponse>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        [HttpGet("getall")]
        public async Task<IActionResult> GetList([FromQuery] GetAppOperationClaimsQueryRequest request, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(request, cancellationToken);
            if (result.Success)
            {
                return Ok(result);
            }

            return BadRequest(result.Message);
        }

        ///<summary>
        ///It brings the details according to its id.
        ///</summary>
        ///<remarks>bla bla bla OperationClaims</remarks>
        ///<return>OperationClaims List</return>
        ///<response code="200"></response>  
        [Produces("application/json", "text/plain")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResponseBase<GetAppOperationClaimQueryResponse>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        [HttpGet("getbyid")]
        public async Task<IActionResult> GetByid(GetAppOperationClaimQueryRequest request, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(request, cancellationToken);
            if (result.Success)
            {
                return Ok(result.Data);
            }

            return BadRequest(result.Message);
        }

        /// <summary>
        /// Add Operation Claim.
        /// </summary>
        /// <param name="CreateAppOperationClaim"></param>
        /// <returns></returns>
        [Consumes("application/json")]
        [Produces("application/json", "text/plain")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResponseBase<CreateAppOperationClaimCommandResponse>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        [HttpPost]
        public async Task<IActionResult> Add([FromBody] CreateAppOperationClaimCommandRequest CreateAppOperationClaim, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(CreateAppOperationClaim, cancellationToken);
            if (result.Success)
            {
                return Ok(result.Message);
            }

            return BadRequest(result.Message);
        }


        /// <summary>
        /// Update OperationClaim .
        /// </summary>
        /// <param name="updateOperationClaim"></param>
        /// <returns></returns>
        [Consumes("application/json")]
        [Produces("application/json", "text/plain")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResponseBase<UpdateAppOperationClaimCommandResponse>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        [HttpPut]
        public async Task<IActionResult> Update([FromBody] UpdateAppOperationClaimCommandRequest updateOperationClaim, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(updateOperationClaim, cancellationToken);
            if (result.Success)
            {
                return Ok(result.Message);
            }

            return BadRequest(result.Message);
        }

        /// <summary>
        /// Delete Operation Claim.
        /// </summary>
        /// <param name="deleteOperationClaim"></param>
        /// <returns></returns>
        [Consumes("application/json")]
        [Produces("application/json", "text/plain")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResponseBase<DeleteAppOperationClaimCommandResponse>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        [HttpDelete]
        public async Task<IActionResult> Delete([FromBody] DeleteAppOperationClaimCommandRequest deleteOperationClaim, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(deleteOperationClaim, cancellationToken);
            if (result.Success)
            {
                return Ok(result.Message);
            }

            return BadRequest(result.Message);
        }
    }
}
