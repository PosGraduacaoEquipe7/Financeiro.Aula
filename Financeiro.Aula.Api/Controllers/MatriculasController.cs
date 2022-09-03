using Financeiro.Aula.Api.Models.Contratos.Mappers;
using Financeiro.Aula.Domain.Commands.Matriculas.GerarContratoDaMatricula;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Financeiro.Aula.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MatriculasController : ControllerBase
    {
        private readonly IMediator _mediator;

        public MatriculasController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> IncluirContrato([FromBody] GerarContratoDaMatriculaCommand request)
        {
            var contrato = await _mediator.Send(request);

            if (contrato == null)
                return BadRequest();

            var response = IncluirContratoResponseMapper.Map(contrato);

            return Created(contrato.Id.ToString(), response);
        }
    }
}