namespace Financeiro.Aula.Domain.Entities
{
    public class Curso
    {
        public const long CURSO_PADRAO_ID = 1;

        public long Id { get; private set; }

        public string Descricao { get; private set; }

        public double CargaHoraria { get; private set; }

        public double ValorBruto { get; private set; }

        public Curso(long id, string descricao, double cargaHoraria, double valorBruto)
        {
            Id = id;
            Descricao = descricao;
            CargaHoraria = cargaHoraria;
            ValorBruto = valorBruto;
        }
    }
}