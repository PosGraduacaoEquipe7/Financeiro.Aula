using Financeiro.Boleto.Domain.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Financeiro.Boleto.Api.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class BoletosController : ControllerBase
    {
        private readonly IBoletoService _boletoService;

        public BoletosController(IBoletoService boletoService)
        {
            _boletoService = boletoService;
        }

        [HttpGet("{id}/pdf")]
        public async Task<IActionResult> ObterPdfBoleto([FromRoute] Guid id)
        {
            var pdf = await _boletoService.ObterPdfBoleto(id);

            if (pdf is null)
                return NotFound();

            return Ok(pdf);
        }
    }
}