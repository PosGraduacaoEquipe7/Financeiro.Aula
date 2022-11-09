namespace Financeiro.Boleto.Domain.Interfaces.Repositories
{
    public interface IBoletoRepository
    {
        Task<Entities.Boleto?> ObterBoleto(Guid id);
        Task IncluirBoleto(Entities.Boleto boleto);
    }
}