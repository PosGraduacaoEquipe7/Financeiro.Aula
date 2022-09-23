using Financeiro.Aula.Api.Models.Parcelas.Mappers;
using Financeiro.Aula.Domain.Commands.Parcelas.GerarBoletoParcela;
using Financeiro.Aula.Domain.Commands.Parcelas.ListarParcelas;
using Financeiro.Aula.Domain.Commands.Parcelas.ObterPdfBoletoParcela;
using Financeiro.Aula.Domain.Commands.Parcelas.PagarParcela;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Financeiro.Aula.Api.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ParcelasController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ParcelasController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("/api/Contratos/{contratoId}/parcelas")]
        public async Task<IActionResult> ListarParcelas([FromRoute] long contratoId)
        {
            var parcelas = await _mediator.Send(new ListarParcelasCommand(contratoId));

            var parcelasLista = ListarParcelaResponseMapper.Map(parcelas);

            return Ok(parcelasLista);
        }

        [HttpPut("{id}/pagar")]
        public async Task<IActionResult> PagarParcela([FromRoute] long id)
        {
            var retorno = await _mediator.Send(new PagarParcelaCommand(id));

            if (!retorno.Sucesso)
                return BadRequest(retorno.Mensagem);

            return Ok();
        }

        [HttpPost("{id}/gerar-boleto")]
        public async Task<IActionResult> GerarBoletoParcela([FromRoute] long id, [FromQuery] bool confirmaSobrescrever)
        {
            var retorno = await _mediator.Send(new GerarBoletoParcelaCommand(id, confirmaSobrescrever));

            if (!retorno.Sucesso)
                return BadRequest(retorno.Mensagem);

            //return Ok(retorno.Pdf);
            return File(retorno.Pdf, "application/pdf");
        }

        [HttpGet("{id}/obter-boleto")]
        public async Task<IActionResult> ObterPdfBoletoParcela([FromRoute] long id)
        {
            var retorno = await _mediator.Send(new ObterPdfBoletoParcelaCommand(id));

            if (!retorno.Sucesso)
                return BadRequest(retorno.Mensagem);

            //return Ok(retorno.Pdf);
            return File(retorno.Pdf, "application/pdf");
        }
    }
}