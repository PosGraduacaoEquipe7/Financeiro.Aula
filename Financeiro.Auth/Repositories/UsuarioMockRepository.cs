using Financeiro.Auth.Entities;
using Financeiro.Auth.Interfaces.Repositories;

namespace Financeiro.Auth.Repositories
{
    public class UsuarioMockRepository : IUsuarioRepository
    {
        private readonly IEnumerable<Usuario> _usuarios;

        public UsuarioMockRepository()
        {
            _usuarios = new[]
            {
                new Usuario(1, "Felipe", "felipejunges@yahoo.com.br", "felipe123", "Admin"),
                new Usuario(2, "Gabriella", "gabigabi@yahoo.com.br", "gabi123", "User"),
            };
        }

        public Task<Usuario?> ObterUsuario(string email, string senha)
        {
            return Task.FromResult(_usuarios.FirstOrDefault(u => u.Email == email && u.Senha == senha));
        }
    }
}
