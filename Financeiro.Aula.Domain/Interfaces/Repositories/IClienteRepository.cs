using Financeiro.Aula.Domain.Entities;

namespace Financeiro.Aula.Domain.Interfaces.Repositories
{
    public interface IClienteRepository
    {
        Task<Cliente?> ObterCliente(long id);
        Task<bool> VerificarCpfExiste(string cpf);
        Task<IEnumerable<Cliente>> ListarClientes(string? nome);
        Task<Cliente?> IncluirCliente(Cliente cliente);
        Task AtualizarCliente(Cliente cliente);
    }
}