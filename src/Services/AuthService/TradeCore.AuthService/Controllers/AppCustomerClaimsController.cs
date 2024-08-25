using MediatR;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using TradeCore.AuthService.Models.Request.Command;
using TradeCore.AuthService.Models.Request.Query;
using TradeCore.AuthService.Models.Response;
using TradeCore.AuthService.Models.Response.Command;
using TradeCore.AuthService.Models.Response.Query;

namespace TradeCore.AuthService.Controllers
{
    [ApiController]
    [Produces("application/json", "text/plain")]
    [Route("api/[controller]")]
    public class AppCustomerClaimsController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<AppCustomerClaimsController> _logger;

        public AppCustomerClaimsController(IMediator mediator, ILogger<AppCustomerClaimsController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        ///<summary>
        ///List UserClaims
        ///</summary>
        ///<remarks>bla bla bla UserClaims</remarks>
        ///<return>UserClaims List</return>
        ///<response code="200"></response>  
        [Produces("application/json", "text/plain")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResponseBase<GetAppCustomerClaimsQueryResponse>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        [HttpGet("getall")]
        public async Task<IActionResult> GetList(GetAppCustomerClaimsQueryRequest request, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(request, cancellationToken);
            if (result.Success)
            {
                _logger.LogInformation("Kullanıcı yetki listesi başarıyla çekilde");

                return Ok(result);
            }
            else
            {
                _logger.LogInformation("Kullanıcı yetki listesi çekilemedi");

                return BadRequest(result.Message);
            }

        }

        ///<summary>
        ///Id sine göre detaylarını getirir.
        ///</summary>
        ///<remarks>bla bla bla </remarks>
        ///<return>UserClaims List</return>
        ///<response code="200"></response>  
        [Produces("application/json", "text/plain")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResponseBase<GetAppCustomerClaimByCustomerIdQueryResponse>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        [HttpGet("getbyuserid")]
        public async Task<IActionResult> GetByUserId(GetAppCustomerClaimByUserIdQueryRequest request, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(request, cancellationToken);
            if (result.Success)
            {
                return Ok(result.Data);
            }

            return BadRequest(result.Message);
        }

        ///<summary>
        ///It brings the details according to its id.
        ///</summary>
        ///<remarks>bla bla bla </remarks>
        ///<return>UserClaims List</return>
        ///<response code="200"></response>  
        [Produces("application/json", "text/plain")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResponseBase<GetAppCustomerClaimOperationClaimByCustomerIdQueryResponse>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        [HttpGet("getoperationclaimbyuserid")]
        public async Task<IActionResult> GetOperationClaimByUserId(GetAppCustomerClaimOperationClaimByCustomerIdQueryRequest request, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(request, cancellationToken);
            if (result.Success)
            {
                return Ok(result.Data);
            }

            return BadRequest(result.Message);
        }

        /// <summary>
        /// Add GroupClaim.
        /// </summary>
        /// <param name="createUserClaim"></param>
        /// <returns></returns>
        [Consumes("application/json")]
        [Produces("application/json", "text/plain")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResponseBase<CreateAppCustomerClaimCommandResponse>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        [HttpPost]
        public async Task<IActionResult> Add([FromBody] CreateAppCustomerClaimCommandRequest createUserClaim, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(createUserClaim, cancellationToken);
            if (result.Success)
            {
                _logger.LogInformation("Kullanıcı yetki ekleme işlemi başarılı");

                return Ok(result.Message);
            }

            return BadRequest(result.Message);
        }

        /// <summary>
        /// Update GroupClaim.
        /// </summary>
        /// <param name="updateUserClaim"></param>
        /// <returns></returns>
        [Consumes("application/json")]
        [Produces("application/json", "text/plain")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResponseBase<UpdateAppCustomerClaimCommandResponse>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        [HttpPut]
        public async Task<IActionResult> Update([FromBody] UpdateAppCustomerClaimCommandRequest updateUserClaim, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(updateUserClaim, cancellationToken);
            if (result.Success)
            {
                _logger.LogInformation("Kullanıcı yetki güncelleme işlemi başarılı");

                return Ok(result.Message);
            }

            return BadRequest(result.Message);
        }

        /// <summary>
        /// Delete GroupClaim.
        /// </summary>
        /// <param name="deleteUserClaim"></param>
        /// <returns></returns>
        [Consumes("application/json")]
        [Produces("application/json", "text/plain")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResponseBase<DeleteAppCustomerClaimCommandResponse>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        [HttpDelete]
        public async Task<IActionResult> Delete([FromBody] DeleteAppCustomerClaimCommandRequest deleteUserClaim, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(deleteUserClaim, cancellationToken);
            if (result.Success)
            {
                _logger.LogInformation("Kullanıcı yetki silme işlemi başarılı");

                return Ok(result.Message);
            }

            return BadRequest(result.Message);
        }
    }
}
