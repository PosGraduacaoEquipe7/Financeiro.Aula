namespace Financeiro.Aula.Api.Models.Contratos.Responses
{
    public class IncluirContratoResponse
    {
        public long Id { get; init; }

        public DateTime DataEmissao { get; init; }

        public double ValorTotal { get; init; }

        public long ClienteId { get; init; }
    }
}