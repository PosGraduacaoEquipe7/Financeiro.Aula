using Financeiro.Aula.Domain.Entities;
using Financeiro.Aula.Domain.Interfaces.Repositories;

namespace Financeiro.Aula.Infra.Repositories
{
    public class CursoMockRepository : ICursoRepository
    {
        public async Task<Curso?> ObterCursoPadrao()
        {
            var curso = new Curso(
                id: 1,
                descricao: "Nutrição",
                cargaHoraria: 100,
                valorBruto: 5000
            );

            return await Task.FromResult<Curso>(curso);
        }
    }
}