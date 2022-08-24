namespace Financeiro.Aula.Domain.Entities
{
    public class Contrato
    {
        public long Id { get; private set; }

        public DateTime DataEmissao { get; private set; }

        public decimal ValorTotal { get; private set; }

        public bool Cancelado { get; private set; }

        public long ClienteId { get; private set; }

        public virtual Cliente? Cliente { get; private set; }

        public virtual Turma Turma { get; private set; } = null!;

        public virtual ICollection<Parcela> Parcelas { get; private set; }

        private Contrato() : this(default, default, default, default)
        {
        }

        public Contrato(long id, DateTime dataEmissao, decimal valorTotal, long clienteId)
        {
            Id = id;
            DataEmissao = dataEmissao;
            ValorTotal = valorTotal;
            ClienteId = clienteId;

            Cancelado = false;

            Parcelas = new HashSet<Parcela>();
        }

        public void Cancelar()
        {
            Cancelado = true;
        }

        public void AgregarTurma(Turma turma)
        {
            Turma = turma;
        }
    }
}