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

        public async Task<IEnumerable<Parcela>> ListarParcelas()
        {
            return await _context.Parcelas.ToListAsync();
        }

        public async Task IncluirParcela(Parcela parcela)
        {
            await _context.Parcelas.AddAsync(parcela);
            await _context.SaveChangesAsync();
        }
    }
}