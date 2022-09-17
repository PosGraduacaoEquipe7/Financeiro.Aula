using Financeiro.Aula.Domain.Entities;

namespace Financeiro.Aula.Domain.Interfaces.Repositories
{
    public interface IParcelaRepository
    {
        Task<Parcela?> ObterParcela(long id);
        Task<IEnumerable<Parcela>> ListarParcelas(long? contratoId);
        Task IncluirParcela(Parcela parcela);
        Task IncluirParcelas(IEnumerable<Parcela> parcelas);
        Task AlterarParcela(Parcela parcela);
    }
}