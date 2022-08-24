namespace Financeiro.Aula.Domain.Entities
{
    public class Turma
    {
        public long Id { get; private set; }

        public string Numero { get; private set; }

        public string Horario { get; private set; }

        public Curso Curso { get; private set; }

        public DateTime DataInicio { get; private set; }

        public DateTime DataTermino { get; private set; }

        public Turma(long id, string numero, string horario, Curso curso, DateTime dataInicio, DateTime dataTermino)
        {
            Id = id;
            Numero = numero;
            Horario = horario;
            Curso = curso;
            DataInicio = dataInicio;
            DataTermino = dataTermino;
        }
    }
}