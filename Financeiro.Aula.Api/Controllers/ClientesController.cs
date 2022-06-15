using Financeiro.Aula.Domain.Commands.Clientes.IncluirCliente;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Financeiro.Aula.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClientesController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ClientesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> IncluirCliente(IncluirClienteCommand request)
        {
            var cliente = await _mediator.Send(request);

            if (cliente == null)
                return BadRequest();

            return Created(cliente.Id.ToString(), cliente);
        }
    }
}