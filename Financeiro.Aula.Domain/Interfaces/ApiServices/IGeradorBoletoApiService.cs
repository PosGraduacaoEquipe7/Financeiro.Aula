using Financeiro.Aula.Domain.DTOs;
using Financeiro.Aula.Domain.Entities;

namespace Financeiro.Aula.Domain.Interfaces.ApiServices
{
    public interface IGeradorBoletoApiService
    {
        Task<(bool Sucesso, byte[] Pdf)> ObterPdfBoleto(Parcela parcela);
        Task<CriacaoBoletoDto> GerarBoleto(Parcela parcela);
    }
}