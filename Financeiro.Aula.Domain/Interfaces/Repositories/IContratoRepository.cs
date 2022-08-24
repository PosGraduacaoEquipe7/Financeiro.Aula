using Financeiro.Aula.Domain.Entities;

namespace Financeiro.Aula.Domain.Interfaces.Repositories
{
    public interface IContratoRepository
    {
        Task<Contrato?> ObterContrato(long id);
        Task<Contrato?> ObterContratoComParcelasECliente(long id);
        Task<IEnumerable<Contrato>> ListarContratos(long? clienteId);
        Task IncluirContrato(Contrato contrato);
        Task AlterarContrato(Contrato contrato);
    }
}