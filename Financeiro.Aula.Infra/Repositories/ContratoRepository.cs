using Financeiro.Aula.Domain.Entities;
using Financeiro.Aula.Domain.Interfaces.Repositories;
using Financeiro.Aula.Infra.Context;

namespace Financeiro.Aula.Infra.Repositories
{
    public class ContratoRepository : IContratoRepository
    {
        private readonly FinanceiroDb _context;

        public ContratoRepository(FinanceiroDb context)
        {
            _context = context;
        }

        public async Task IncluirContrato(Contrato contrato)
        {
            await _context.Contratos.AddAsync(contrato);
            await _context.SaveChangesAsync();
        }
    }
}