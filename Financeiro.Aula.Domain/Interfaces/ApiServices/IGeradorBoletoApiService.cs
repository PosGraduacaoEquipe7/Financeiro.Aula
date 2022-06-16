using Financeiro.Aula.Domain.Entities;

namespace Financeiro.Aula.Domain.Interfaces.ApiServices
{
    public interface IGeradorBoletoApiService
    {
        Task<(bool Sucesso, string Numero, string Token, byte[] Pdf)> GerarBoleto(Parcela parcela);
    }
}