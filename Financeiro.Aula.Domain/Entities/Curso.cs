namespace Financeiro.Aula.Domain.Entities
{
    public class Curso
    {
        public long Id { get; private set; }

        public string Descricao { get; private set; }

        public double CargaHoraria { get; private set; }

        public Curso(long id, string descricao, double cargaHoraria)
        {
            Id = id;
            Descricao = descricao;
            CargaHoraria = cargaHoraria;
        }
    }
}