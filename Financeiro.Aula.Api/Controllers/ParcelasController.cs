using Financeiro.Aula.Domain.Commands.Parcelas.ListarParcelas;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Financeiro.Aula.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ParcelasController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ParcelasController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> ListarParcelas()
        {
            var parcelas = await _mediator.Send(new ListarParcelasCommand());

            return Ok(parcelas);
        }
    }
}