using Financeiro.Auth.Entities;

namespace Financeiro.Auth.Interfaces.Repositories
{
    public interface IAcessoRepository
    {
        Task<Acesso?> ObterPeloId(long id);
        Task Incluir(Acesso acesso);
        Task Alterar(Acesso acesso);
    }
}