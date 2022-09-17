using Financeiro.Aula.Domain.Entities;
using Financeiro.Aula.Domain.Interfaces.Repositories;
using Financeiro.Aula.Infra.Context;
using Microsoft.EntityFrameworkCore;

namespace Financeiro.Aula.Infra.Repositories
{
    public class ParcelaRepository : IParcelaRepository
    {
        private readonly FinanceiroDb _context;

        public ParcelaRepository(FinanceiroDb context)
        {
            _context = context;
        }

        public async Task<Parcela?> ObterParcela(long id)
        {
            return await _context.Parcelas
                            .Include(p => p.Contrato)
                            .ThenInclude(p => p.Cliente)
                            .Where(p => p.Id == id)
                            .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Parcela>> ListarParcelas(long? contratoId)
        {
            return await _context.Parcelas
                            .Where(p =>
                                contratoId == null || p.ContratoId == contratoId)
                            .ToListAsync();
        }

        public async Task IncluirParcela(Parcela parcela)
        {
            await _context.Parcelas.AddAsync(parcela);
            await _context.SaveChangesAsync();
        }

        public async Task IncluirParcelas(IEnumerable<Parcela> parcelas)
        {
            await _context.Parcelas.AddRangeAsync(parcelas);
            await _context.SaveChangesAsync();
        }

        public async Task AlterarParcela(Parcela parcela)
        {
            await Task.Run(() => _context.Parcelas.Update(parcela));
            await _context.SaveChangesAsync();
        }
    }
}