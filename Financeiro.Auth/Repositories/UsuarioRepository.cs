using Financeiro.Auth.Context;
using Financeiro.Auth.Entities;
using Financeiro.Auth.Interfaces.Repositories;

namespace Financeiro.Auth.Repositories
{
    public class UsuarioRepository : IUsuarioRepository
    {
        private readonly AuthDb _db;

        public UsuarioRepository(AuthDb db)
        {
            _db = db;
        }

        public Task<Usuario?> ObterUsuarioPeloEmail(string email)
        {
            return Task.FromResult(_db.Usuarios.FirstOrDefault(u => u.Email == email));
        }
    }
}
