using Financeiro.Boleto.Domain.DTOs;
using Financeiro.Boleto.Domain.Interfaces.ApiServices;
using Financeiro.Boleto.Domain.Interfaces.Repositories;
using Financeiro.Boleto.Domain.Interfaces.Services;

namespace Financeiro.Boleto.Domain.Services
{
    public class BoletoService : IBoletoService
    {
        private readonly IGeradorBoletoApiService _geradorBoletoApiService;
        private readonly IBoletoRepository _boletoRepository;

        public BoletoService(IGeradorBoletoApiService geradorBoletoApiService, IBoletoRepository boletoRepository)
        {
            _geradorBoletoApiService = geradorBoletoApiService;
            _boletoRepository = boletoRepository;
        }

        public async Task RegistrarBoleto(BoletoGerarDto boletoDto)
        {
            var boleto = await _geradorBoletoApiService.GerarBoleto(boletoDto);

            if (boleto is null)
                return;

            await _boletoRepository.IncluirBoleto(boleto);
        }
    }
}