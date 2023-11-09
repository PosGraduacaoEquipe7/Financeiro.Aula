using Financeiro.Auth.Entities;

namespace Financeiro.Auth.Interfaces.Services
{
    public interface IAcessoService
    {
        Task<Acesso> GerarAcesso(Usuario usuario);
        Task AtualizarRefreshToken(Acesso acesso);
    }
}