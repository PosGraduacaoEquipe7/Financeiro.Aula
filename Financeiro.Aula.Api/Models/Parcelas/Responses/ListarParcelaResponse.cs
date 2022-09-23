namespace Financeiro.Aula.Api.Models.Parcelas.Responses
{
    public class ListarParcelaResponse
    {
        public long Id { get; init; }

        public int Sequencial { get; init; }

        public double Valor { get; init; }

        public DateTime DataVencimento { get; init; }

        public bool TemBoleto { get; init; }
    }
}