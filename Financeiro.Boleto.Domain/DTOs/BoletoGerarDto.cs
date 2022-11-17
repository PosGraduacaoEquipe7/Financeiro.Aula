using Financeiro.Boleto.Domain.ValueObjects;

namespace Financeiro.Boleto.Domain.DTOs
{
    public record BoletoGerarDto
    {
        public Guid TokenRetorno;

        public string IdentificadorContrato = string.Empty;

        public DateTime DataVencimento;

        public double Valor;

        public ClienteBoletoGerarDto Cliente = null!;
    }

    public record ClienteBoletoGerarDto
    {
        public string Nome = string.Empty;

        public string Cpf = string.Empty;

        public Endereco Endereco = null!;
    }
}