using Financeiro.Aula.Domain.Entities;

namespace Financeiro.Aula.Domain.Interfaces.Repositories
{
    public interface ITurmaRepository
    {
        Task<Turma?> ObterTurmaPadrao();
    }
}