using Financeiro.Auth.Commands.Login;
using Financeiro.Auth.Models.Login;
using MediatR;
using Microsoft.AspNetCore.Mvc;

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
        public async Task<ActionResult<LoginResponse>> Login(LoginRequest request)
        {
            var response = await _mediator.Send(new LoginCommand(request));

            if (!response.Sucesso)
                return BadRequest(response);

            return Ok(response);
        }
    }
}
