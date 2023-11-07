using Financeiro.Aula.Domain.Interfaces.Services.CEPs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Financeiro.Aula.Api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class CepsController : ControllerBase
    {
        private readonly ICEPService _cepService;

        public CepsController(ICEPService cepService)
        {
            _cepService = cepService;
        }

        [HttpGet("/api/Cep/{cep}/endereco")]
        public async Task<IActionResult> ObterEnderecoPeloCEP(string cep)
        {
            var endereco = await _cepService.BuscarCEP(cep);

            if (endereco == null)
                return NotFound();

            return Ok(endereco);
        }
    }
}