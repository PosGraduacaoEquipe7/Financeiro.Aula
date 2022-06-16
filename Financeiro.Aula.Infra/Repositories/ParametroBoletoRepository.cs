using Financeiro.Aula.Domain.Entities;
using Financeiro.Aula.Domain.Interfaces.Repositories;
using Financeiro.Aula.Infra.Context;
using Microsoft.EntityFrameworkCore;

namespace Financeiro.Aula.Infra.Repositories
{
    public class ParametroBoletoRepository : IParametroBoletoRepository
    {
        private const long PARAMETRO_BOLETO_PADRAO = 1;

        private readonly FinanceiroDb _context;

        public ParametroBoletoRepository(FinanceiroDb context)
        {
            _context = context;
        }

        public async Task<ParametroBoleto?> ObterParametrosBoleto()
        {
            return await _context.ParametrosBoleto
                            .Where(p => p.Id == PARAMETRO_BOLETO_PADRAO)
                            .FirstOrDefaultAsync();
        }
    }
}