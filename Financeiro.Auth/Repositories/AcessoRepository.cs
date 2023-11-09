using Financeiro.Auth.Context;
using Financeiro.Auth.Entities;
using Financeiro.Auth.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Financeiro.Auth.Repositories
{
    public class AcessoRepository : IAcessoRepository
    {
        private readonly AuthDb _db;

        public AcessoRepository(AuthDb db)
        {
            _db = db;
        }

        public Task<Acesso?> ObterPeloId(long id)
        {
            return _db.Acessos
                .Include(a => a.Usuario)
                .FirstOrDefaultAsync(a => a.Id == id);
        }

        public async Task Incluir(Acesso acesso)
        {
            await _db.Acessos.AddAsync(acesso);
            await _db.SaveChangesAsync();
        }

        public async Task Alterar(Acesso acesso)
        {
            await Task.Run(() => _db.Update(acesso));
            await _db.SaveChangesAsync();
        }
    }
}