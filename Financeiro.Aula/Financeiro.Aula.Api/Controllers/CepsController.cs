using Financeiro.Aula.Domain.Interfaces.Services.CEPs;
using Financeiro.Aula.Domain.ValueObjects;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Financeiro.Aula.Api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class CepsController : ControllerBase
    {
        private readonly ICepService _cepService;

        public CepsController(ICepService cepService)
        {
            _cepService = cepService;
        }

        [HttpGet("/api/Cep/{cep}/endereco")]
        [ProducesResponseType(typeof(Endereco), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Endereco>> ObterEnderecoPeloCEP(string cep)
        {
            var endereco = await _cepService.BuscarCEP(cep);

            if (endereco == null)
                return NotFound();

            return Ok(endereco);
        }
    }
}