using Financeiro.Aula.Domain.DTOs;
using Financeiro.Aula.Domain.Entities;

namespace Financeiro.Aula.Domain.Interfaces.DomainServices
{
    public interface IClienteService
    {
        Task<Cliente> IncluirOuAlterarCliente(ClienteAtualizacaoDto clienteDto);
    }
}