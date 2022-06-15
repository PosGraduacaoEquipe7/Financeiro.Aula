using Financeiro.Aula.Domain.Entities;

namespace Financeiro.Aula.Domain.Interfaces.Repositories
{
    public interface IParcelaRepository
    {
        Task<IEnumerable<Parcela>> ListarParcelas();
        Task IncluirParcela(Parcela parcela);
    }
}