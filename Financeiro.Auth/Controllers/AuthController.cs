using Financeiro.Auth.Commands.Login;
using Financeiro.Auth.Commands.RefreshToken;
using Financeiro.Auth.Models.Login;
using Financeiro.Auth.Models.RefreshToken;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Financeiro.Auth.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AuthController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        [ProducesResponseType(typeof(LoginResponse), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(LoginResponse), (int)HttpStatusCode.BadRequest)]
        public async Task<ActionResult<LoginResponse>> Login([FromBody] LoginRequest request)
        {
            var response = await _mediator.Send(new LoginCommand(request));

            if (!response.Sucesso)
                return BadRequest(response);

            return Ok(response);
        }

        [HttpPut]
        [ProducesResponseType(typeof(RefreshTokenResponse), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(RefreshTokenResponse), (int)HttpStatusCode.BadRequest)]
        public async Task<ActionResult<RefreshTokenResponse>> RefreshToken([FromBody] RefreshTokenRequest request)
        {
            var response = await _mediator.Send(new RefreshTokenCommand(request));

            if (!response.Sucesso)
                return BadRequest(response);

            return Ok(response);
        }
    }
}