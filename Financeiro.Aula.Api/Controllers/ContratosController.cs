using Financeiro.Aula.Api.Models.Contratos.Mappers;
using Financeiro.Aula.Domain.Commands.Contratos.CancelarContrato;
using Financeiro.Aula.Domain.Commands.Contratos.ImprimirContrato;
using Financeiro.Aula.Domain.Commands.Contratos.IncluirContrato;
using Financeiro.Aula.Domain.Commands.Contratos.ListarContratos;
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

        [HttpGet]
        public async Task<IActionResult> ListarContratos([FromQuery] long? clienteId)
        {
            var contratos = await _mediator.Send(new ListarContratosCommand(clienteId));

            return Ok(contratos);
        }

        [HttpPost]
        public async Task<IActionResult> IncluirContrato([FromBody] IncluirContratoCommand request)
        {
            var contrato = await _mediator.Send(request);

            if (contrato == null)
                return BadRequest();

            var response = IncluirContratoResponseMapper.Map(contrato);

            return Created(contrato.Id.ToString(), response);
        }

        [HttpPut("{id}/cancelar")]
        public async Task<IActionResult> CancelarContrato([FromRoute] long id)
        {
            var retorno = await _mediator.Send(new CancelarContratoCommand(id));

            if (!retorno.Sucesso)
                return BadRequest(retorno.Mensagem);

            return Ok();
        }

        [HttpGet("{id}/termos")]
        public async Task<IActionResult> GerarTermoAceite([FromRoute] long id)
        {
            var retorno = await _mediator.Send(new ImprimirContratoCommand(id));

            if (!retorno.Sucesso)
                return BadRequest(retorno.Mensagem);

            //return Ok(retorno.Contrato);
            return File(retorno.Contrato, "application/pdf");
        }
    }
}