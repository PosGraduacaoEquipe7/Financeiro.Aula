namespace Financeiro.Aula.Domain.Entities
{
    public class Contrato
    {
        public long Id { get; private set; }

        public DateTime DataEmissao { get; private set; }

        public double ValorTotal { get; private set; }

        public bool Cancelado { get; private set; }

        public long ClienteId { get; private set; }

        public virtual Cliente? Cliente { get; private set; }

        public virtual Turma Turma { get; private set; } = null!;

        public DateTime? DataHoraAceiteContrato { get; private set; }

        public bool ContratoAceite => DataHoraAceiteContrato.HasValue;

        public virtual ICollection<Parcela> Parcelas { get; private set; }

        private Contrato() : this(default, default, default, default)
        {
        }

        public Contrato(long id, DateTime dataEmissao, double valorTotal, long clienteId)
        {
            Id = id;
            DataEmissao = dataEmissao;
            ValorTotal = valorTotal;
            ClienteId = clienteId;

            Cancelado = false;

            Parcelas = new HashSet<Parcela>();
        }

        public void AceitarContrato()
        {
            DataHoraAceiteContrato = DateTime.Now;
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