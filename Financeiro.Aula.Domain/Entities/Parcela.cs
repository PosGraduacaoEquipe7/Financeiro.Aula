﻿namespace Financeiro.Aula.Domain.Entities
{
    public class Parcela
    {
        public long Id { get; private set; }

        public int Sequencial { get; private set; }

        public double Valor { get; private set; }

        public DateTime DataVencimento { get; private set; }

        public DateTime? DataPagamento { get; private set; }

        public long ContratoId { get; private set; }

        public virtual Contrato? Contrato { get; private set; }

        public string? NumeroBoleto { get; private set; }

        public string? ChaveBoleto { get; private set; }

        public bool Paga => DataPagamento is not null;

        public bool TemBoleto => !string.IsNullOrEmpty(NumeroBoleto);

        private Parcela()
        {
        }

        public Parcela(long id, int sequencial, double valor, DateTime dataVencimento, long contratoId)
        {
            Id = id;
            Sequencial = sequencial;
            Valor = valor;
            DataVencimento = dataVencimento;
            ContratoId = contratoId;
        }

        public void Pagar()
        {
            DataPagamento = DateTime.Now.Date;
        }

        public void RegistrarBoleto(string numeroBoleto, string chaveBoleto)
        {
            NumeroBoleto = numeroBoleto;
            ChaveBoleto = chaveBoleto;
        }
    }
}