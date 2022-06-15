namespace Financeiro.Aula.Domain.Entities
{
    public class Parcela
    {
        public long Id { get; private set; }

        public int Sequencial { get; private set; }

        public decimal Valor { get; private set; }

        public DateTime DataVencimento { get; private set; }

        public long ContratoId { get; private set; }

        public virtual Contrato? Contrato { get; private set; }

        private Parcela()
        {
        }

        public Parcela(long id, int sequencial, decimal valor, DateTime dataVencimento, long contratoId)
        {
            Id = id;
            Sequencial = sequencial;
            Valor = valor;
            DataVencimento = dataVencimento;
            ContratoId = contratoId;
        }
    }
}