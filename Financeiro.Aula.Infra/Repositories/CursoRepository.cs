using Financeiro.Aula.Domain.Entities;
using Financeiro.Aula.Domain.Interfaces.Repositories;
using Financeiro.Aula.Infra.Context;
using Microsoft.EntityFrameworkCore;

namespace Financeiro.Aula.Infra.Repositories
{
    public class CursoRepository : ICursoRepository
    {
        private readonly FinanceiroDb _context;

        public CursoRepository(FinanceiroDb context)
        {
            _context = context;
        }

        public Task<Curso?> ObterCursoPadrao()
        {
            return _context.Cursos.FirstOrDefaultAsync(c => c.Id == Curso.CURSO_PADRAO_ID);
        }
    }
}