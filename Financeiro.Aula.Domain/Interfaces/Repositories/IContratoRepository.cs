using Financeiro.Aula.Domain.Entities;

namespace Financeiro.Aula.Domain.Interfaces.Repositories
{
    public interface IContratoRepository
    {
        Task IncluirContrato(Contrato contrato);
    }
}