using Financeiro.Aula.Domain.Entities;

namespace Financeiro.Aula.Domain.Interfaces.Repositories
{
    public interface IClienteRepository
    {
        Task<IEnumerable<Cliente>> ListarClientes();
        Task<Cliente?> IncluirCliente(Cliente cliente);
    }
}