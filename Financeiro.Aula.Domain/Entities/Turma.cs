namespace Financeiro.Aula.Domain.Entities
{
    public class Turma
    {
        public long Id { get; private set; }

        public string Numero { get; private set; }

        public string Horario { get; private set; }

        public long CursoId { get; private set; }
        public Curso Curso { get; private set; } = default!;

        public DateTime DataInicio { get; private set; }

        public DateTime DataTermino { get; private set; }

        public string Descricao => $"{Curso.Descricao} ({DataInicio:dd/MMM} - {DataTermino:dd/MMM})";

        public Turma(long id, string numero, string horario, long cursoId, DateTime dataInicio, DateTime dataTermino)
        {
            Id = id;
            Numero = numero;
            Horario = horario;
            CursoId = cursoId;
            DataInicio = dataInicio;
            DataTermino = dataTermino;
        }
    }
}