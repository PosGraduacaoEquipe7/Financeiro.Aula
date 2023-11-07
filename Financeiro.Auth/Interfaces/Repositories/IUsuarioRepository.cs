using Financeiro.Auth.Entities;

namespace Financeiro.Auth.Interfaces.Repositories
{
    public interface IUsuarioRepository
    {
        Task<Usuario?> ObterUsuario(string email, string senha);
    }
}
