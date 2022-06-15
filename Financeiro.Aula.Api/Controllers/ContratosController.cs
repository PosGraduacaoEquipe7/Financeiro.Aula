using Financeiro.Aula.Domain.Commands.Contratos.IncluirContrato;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Financeiro.Aula.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContratosController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ContratosController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> IncluirCliente(IncluirContratoCommand request)
        {
            var contrato = await _mediator.Send(request);

            if (contrato == null)
                return BadRequest();

            return Created(contrato.Id.ToString(), contrato);
        }
    }
}