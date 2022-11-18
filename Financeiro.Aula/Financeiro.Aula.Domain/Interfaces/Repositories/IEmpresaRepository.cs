using Financeiro.Aula.Domain.Entities;

namespace Financeiro.Aula.Domain.Interfaces.Repositories
{
    public interface IEmpresaRepository
    {
        Task<Empresa?> ObterEmpresaPadrao();
    }
}