using Financeiro.Aula.Domain.Commands.Clientes.AlterarCliente;
using Financeiro.Aula.Domain.Commands.Clientes.IncluirCliente;
using Financeiro.Aula.Domain.Commands.Clientes.ListarClientes;
using Financeiro.Aula.Domain.Entities;
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
        [ProducesResponseType(typeof(IEnumerable<ListarClientesCommandResult>), StatusCodes.Status200OK)]
        public async Task<ActionResult<ListarClientesCommandResult>> ListarClientes([FromQuery] string? nome)
        {
            var clientes = await _mediator.Send(new ListarClientesCommand(nome));

            return Ok(clientes);
        }

        [HttpPost]
        [ProducesResponseType(typeof(Cliente), StatusCodes.Status201Created)]
        public async Task<ActionResult<Cliente>> IncluirCliente([FromBody] IncluirClienteCommand request)
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