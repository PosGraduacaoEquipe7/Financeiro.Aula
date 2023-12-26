using Financeiro.Boleto.Domain.DTOs;

namespace Financeiro.Boleto.Domain.Interfaces.ApiServices
{
    public interface IGeradorBoletoApiService
    {
        Task<byte[]?> ObterPdfBoleto(string chaveBoleto);
        Task<string?> GerarTokenBoleto(BoletoGerarDto boleto, string numeroBoleto);
    }
}