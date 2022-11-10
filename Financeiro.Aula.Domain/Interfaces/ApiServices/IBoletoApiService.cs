namespace Financeiro.Aula.Domain.Interfaces.ApiServices
{
    public interface IBoletoApiService
    {
        Task<string?> ObterPdfBoleto(long parcelaId);
    }
}