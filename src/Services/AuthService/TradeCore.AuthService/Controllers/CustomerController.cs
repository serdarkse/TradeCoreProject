using MediatR;
using Microsoft.AspNetCore.Mvc;
using TradeCore.AuthService.Models.Request.Command.AppCustomer;
using TradeCore.AuthService.Models.Request.Query.AppCustomer;
using TradeCore.AuthService.Models.Response;
using TradeCore.AuthService.Models.Response.Command.AppCustomer;
using TradeCore.AuthService.Models.Response.Query.AppCustomer;

namespace TradeCore.AuthService.Controllers
{
    [ApiController]
    [Produces("application/json", "text/plain")]
    [Route("api/[controller]")]
    public class CustomerController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<AppCustomerClaimsController> _logger;

        public CustomerController(IMediator mediator, ILogger<AppCustomerClaimsController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        ///<summary>
        ///List Users 
        ///</summary>
        ///<remarks>List Users</remarks>
        ///<return>Users List</return>
        ///<response code="200"></response>  
        [Produces("application/json", "text/plain")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResponseBase<AppCustomerGetAllListQueryResponse>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        [HttpGet("getall")]
        public async Task<IActionResult> GetList([FromQuery] AppCustomerGetAllListQueryRequest request, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(request, cancellationToken);
            if (result.Success)
            {
                _logger.LogInformation("Kullanıcı listesi çekme işlemi başarılı");

                return Ok(result);
            }
            else
                return BadRequest(result.Message);
        }

        ///<summary>
        ///It brings the details according to its id.
        ///</summary>
        ///<remarks>bla bla bla </remarks>
        ///<return>Users List</return>
        ///<response code="200"></response>
        [Produces("application/json", "text/plain")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResponseBase<AppCustomerGetByIdQueryResponse>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        [HttpGet("getbyid")]
        public async Task<IActionResult> GetById([FromQuery] AppCustomerGetByIdQueryRequest request, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(request, cancellationToken);
            if (result.Success)
            {
                return Ok(result.Data);
            }

            return BadRequest(result.Message);
        }

        /// <summary>
        /// Add User.
        /// </summary>
        /// <param name="createUser"></param>
        /// <returns></returns>
        [Produces("application/json", "text/plain")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResponseBase<CreateAppCustomerCommandResponse>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        [HttpPost("add")]
        public async Task<IActionResult> Add([FromBody] CreateAppCustomerCommandRequest createUser, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(createUser, cancellationToken);
            if (result.Success)
            {
                _logger.LogInformation("Kullanıcı ekleme işlemi başarılı");

                return Ok(result.Message);
            }

            return BadRequest(result.Message);
        }

        /// <summary>
        /// Update User.
        /// </summary>
        /// <param name="updateUser"></param>
        /// <returns></returns>
        [Consumes("application/json")]
        [Produces("application/json", "text/plain")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResponseBase<UpdateAppCustomerCommandResponse>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        [HttpPut("update")]
        public async Task<IActionResult> Update([FromBody] UpdateAppCustomerCommandRequest updateUser, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(updateUser, cancellationToken);
            if (result.Success)
            {
                _logger.LogInformation("Kullanıcı güncelleme işlemi başarılı");

                return Ok(result.Message);
            }

            return BadRequest(result.Message);
        }

        /// <summary>
        /// Delete User.
        /// </summary>
        /// <param name="deleteUser"></param>
        /// <returns></returns>
        [Consumes("application/json")]
        [Produces("application/json", "text/plain")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResponseBase<DeleteAppCustomerCommandResponse>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        [HttpDelete]
        public async Task<IActionResult> Delete([FromBody] DeleteAppCustomerCommandRequest deleteUser, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(deleteUser, cancellationToken);
            if (result.Success)
            {
                _logger.LogInformation("Kullanıcı silme işlemi başarılı");

                return Ok(result.Message);
            }

            return BadRequest(result.Message);
        }

        ///<summary>
        ///It brings the details according to its id.
        ///</summary>
        ///<remarks>bla bla bla </remarks>
        ///<return>Users List</return>
        ///<response code="200"></response>
        [Consumes("application/json")]
        [Produces("application/json", "text/plain")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResponseBase<AppCustomerGetByEmailQueryResponse>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        [HttpPost("getbyemail")]
        public async Task<IActionResult> GetByEmail([FromBody] AppCustomerGetByEmailQueryRequest request, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(request, cancellationToken);
            if (result.Success)
            {
                return Ok(result);
            }

            return BadRequest(result);
        }
    }
}
