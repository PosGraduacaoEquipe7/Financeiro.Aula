using Financeiro.Boleto.Domain.ValueObjects;

namespace Financeiro.Boleto.Domain.Entities
{
    public class Boleto
    {
        public Guid Id { get; private set; }

        public string NumeroBoleto { get; private set; }

        public string ChaveBoleto { get; private set; }

        public DateTime DataVencimento { get; private set; }

        public double Valor { get; private set; }

        public string Nome { get; private set; }

        public string Cpf { get; private set; }

        public Endereco Endereco { get; private set; }

        private Boleto() : this(string.Empty, string.Empty, DateTime.Now.Date, 0, string.Empty, string.Empty, null!)
        {
        }

        public Boleto(string numeroBoleto, string chaveBoleto, DateTime dataVencimento, double valor, string nome, string cpf, Endereco endereco)
        {
            Id = Guid.NewGuid();
            NumeroBoleto = numeroBoleto;
            ChaveBoleto = chaveBoleto;
            DataVencimento = dataVencimento;
            Valor = valor;
            Nome = nome;
            Cpf = cpf;
            Endereco = endereco;
        }
    }
}