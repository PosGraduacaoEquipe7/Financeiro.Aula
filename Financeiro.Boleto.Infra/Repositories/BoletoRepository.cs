using Financeiro.Boleto.Domain.Interfaces.Repositories;
using Financeiro.Boleto.Infra.Context;

namespace Financeiro.Boleto.Infra.Repositories
{
    public class BoletoRepository : IBoletoRepository
    {
        private readonly BoletoDb _context;

        public BoletoRepository(BoletoDb context)
        {
            _context = context;
        }

        public Task<Domain.Entities.Boleto?> ObterBoleto(Guid id)
        {
            throw new NotImplementedException();
        }

        public async Task IncluirBoleto(Domain.Entities.Boleto boleto)
        {
            await _context.Boletos.AddAsync(boleto);
            await _context.SaveChangesAsync();
        }
    }
}