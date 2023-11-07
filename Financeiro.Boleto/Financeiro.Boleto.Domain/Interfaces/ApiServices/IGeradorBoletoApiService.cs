using Financeiro.Boleto.Domain.DTOs;

namespace Financeiro.Boleto.Domain.Interfaces.ApiServices
{
    public interface IGeradorBoletoApiService
    {
        Task<byte[]?> ObterPdfBoleto(string chaveBoleto);
        Task<Entities.Boleto?> GerarBoleto(BoletoGerarDto boleto);
    }
}