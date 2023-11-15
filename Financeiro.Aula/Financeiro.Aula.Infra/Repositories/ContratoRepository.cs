using Financeiro.Aula.Domain.Entities;
using Financeiro.Aula.Domain.Interfaces.Repositories;
using Financeiro.Aula.Infra.Context;
using Microsoft.EntityFrameworkCore;

namespace Financeiro.Aula.Infra.Repositories
{
    public class ContratoRepository : IContratoRepository
    {
        private readonly FinanceiroDb _context;

        public ContratoRepository(FinanceiroDb context)
        {
            _context = context;
        }

        public async Task<Contrato?> ObterContrato(long id)
        {
            return await _context.Contratos
                            .Include(c => c.Cliente)
                            .Where(c => c.Id == id)
                            .FirstOrDefaultAsync();
        }

        public async Task<Contrato?> ObterContratoComParcelasECliente(long id)
        {
            return await _context.Contratos
                            .Include(c => c.Parcelas)
                            .Include(c => c.Cliente)
#nullable disable
                                .ThenInclude(cl => cl.Endereco)
#nullable restore
                            .Include(c => c.Turma)
                                .ThenInclude(cl => cl.Curso)
                            .Where(c => c.Id == id)
                            .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Contrato>> ListarContratosPeloUsuario(string usuarioId)
        {
            return await _context.Contratos
                            .Include(c => c.Cliente)
                            .Include(c => c.Turma)
                            .ThenInclude(c => c.Curso)
                            .Where(c => c.Cliente!.UsuarioId == usuarioId)
                            .OrderBy(c => c.DataEmissao)
                            .ToListAsync();
        }

        public async Task IncluirContrato(Contrato contrato)
        {
            await _context.Contratos.AddAsync(contrato);
            await _context.SaveChangesAsync();
        }


        public async Task AlterarContrato(Contrato contrato)
        {
            await Task.Run(() => _context.Contratos.Update(contrato));
            await _context.SaveChangesAsync();
        }
    }
}