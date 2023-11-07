using Financeiro.Aula.Domain.Commands.Clientes.AlterarCliente;
using Financeiro.Aula.Domain.Commands.Clientes.IncluirCliente;
using Financeiro.Aula.Domain.Commands.Clientes.ListarClientes;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Financeiro.Aula.Api.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ClientesController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ClientesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> ListarClientes([FromQuery] string? nome)
        {
            var clientes = await _mediator.Send(new ListarClientesCommand(nome));

            return Ok(clientes);
        }

        [HttpPost]
        public async Task<IActionResult> IncluirCliente([FromBody] IncluirClienteCommand request)
        {
            var cliente = await _mediator.Send(request);

            if (cliente == null)
                return BadRequest();

            return Created(cliente.Id.ToString(), cliente);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> AlterarCliente([FromRoute] long id, [FromBody] AlterarClienteCommand request)
        {
            var cliente = await _mediator.Send(request.AgregarId(id));

            if (cliente == null)
                return NotFound();

            return Ok(cliente);
        }
    }
}