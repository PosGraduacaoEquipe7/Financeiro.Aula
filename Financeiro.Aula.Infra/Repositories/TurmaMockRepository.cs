using Financeiro.Aula.Domain.Entities;
using Financeiro.Aula.Domain.Interfaces.Repositories;

namespace Financeiro.Aula.Infra.Repositories
{
    public class TurmaMockRepository : ITurmaRepository
    {
        public async Task<Turma?> ObterTurmaPadrao()
        {
            var turma = new Turma(
                id: 1,
                numero: "2023/1",
                horario: "Seg-Qua-Sex, 19h30-22h30",
                curso: new Curso(
                    id: 1,
                    descricao: "Engenharia de Software",
                    cargaHoraria: 100
                ),
                dataInicio: new DateTime(2023, 3, 13),
                dataTermino: new DateTime(2023, 7, 28)
            );

            return await Task.FromResult<Turma>(turma);
        }
    }
}