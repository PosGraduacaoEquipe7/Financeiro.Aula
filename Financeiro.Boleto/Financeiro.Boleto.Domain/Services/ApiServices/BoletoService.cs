using Financeiro.Boleto.Domain.DTOs;
using Financeiro.Boleto.Domain.Entities;
using Financeiro.Boleto.Domain.Interfaces.ApiServices;
using Financeiro.Boleto.Domain.Interfaces.Repositories;
using Financeiro.Boleto.Domain.Interfaces.Services;
using Microsoft.Extensions.Logging;

namespace Financeiro.Boleto.Domain.Services.ApiServices
{
    public class BoletoService : IBoletoService
    {
        private readonly IGeradorBoletoApiService _geradorBoletoApiService;
        private readonly IBoletoRepository _boletoRepository;
        private readonly ParametroBoleto? _parametroBoleto;
        private readonly ILogger<BoletoService> _logger;

        public BoletoService(
            IGeradorBoletoApiService geradorBoletoApiService,
            IBoletoRepository boletoRepository,
            IParametroBoletoRepository parametroBoletoRepository,
            ILogger<BoletoService> logger)
        {
            _geradorBoletoApiService = geradorBoletoApiService;
            _boletoRepository = boletoRepository;
            _parametroBoleto = parametroBoletoRepository.ObterParametrosBoleto().Result;
            _logger = logger;
        }

        public async Task<string?> ObterPdfBoleto(Guid id)
        {
            var boleto = await _boletoRepository.ObterBoleto(id);
            if (boleto is null)
                return null;

            var pdf = await _geradorBoletoApiService.ObterPdfBoleto(boleto.ChaveBoleto);

            if (pdf is null)
                return null;

            return Convert.ToBase64String(pdf);
        }

        public async Task<Entities.Boleto?> RegistrarBoleto(BoletoGerarDto boletoDto)
        {
            var numeroBoleto = ObterProximoNumeroBoleto();

            if (numeroBoleto is null)
                return null;

            var tokenBoleto = await _geradorBoletoApiService.GerarTokenBoleto(boletoDto, numeroBoleto);

            if (tokenBoleto is null)
                return null;

            var boleto = new Entities.Boleto(
                    numeroBoleto,
                    tokenBoleto,
                    boletoDto.DataVencimento,
                    boletoDto.Valor,
                    boletoDto.Cliente.Nome,
                    boletoDto.Cliente.Cpf,
                    boletoDto.Cliente.Endereco);

            await _boletoRepository.IncluirBoleto(boleto);

            return boleto;
        }

        private string? ObterProximoNumeroBoleto()
        {
            if (_parametroBoleto is null)
            {
                _logger.LogError("Os parâmetros do boleto não estão setados");
                return null;
            }

            return _parametroBoleto.ObterProximoNumeroFormatado();
        }
    }
}