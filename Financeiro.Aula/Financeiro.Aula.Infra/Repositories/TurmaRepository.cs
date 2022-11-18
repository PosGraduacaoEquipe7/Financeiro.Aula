using Financeiro.Aula.Domain.Entities;
using Financeiro.Aula.Domain.Interfaces.Repositories;
using Financeiro.Aula.Infra.Context;
using Microsoft.EntityFrameworkCore;

namespace Financeiro.Aula.Infra.Repositories
{
    public class TurmaRepository : ITurmaRepository
    {
        private readonly FinanceiroDb _context;

        public TurmaRepository(FinanceiroDb context)
        {
            _context = context;
        }

        public Task<Turma?> ObterTurmaDoCurso(long cursoId)
        {
            return _context.Turmas.FirstOrDefaultAsync(t => t.CursoId == cursoId);
        }
    }
}