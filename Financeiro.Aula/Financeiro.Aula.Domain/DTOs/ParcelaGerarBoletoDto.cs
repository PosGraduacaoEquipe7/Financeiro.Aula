using Financeiro.Aula.Domain.Entities;
using Financeiro.Aula.Domain.ValueObjects;

namespace Financeiro.Aula.Domain.DTOs
{
    public record ParcelaGerarBoletoDto
    {
        public Guid TokenRetorno;

        public string IdentificadorContrato;

        public DateTime DataVencimento;

        public double Valor;

        public ClienteParcelaGerarBoletoDto Cliente = null!;

        public ParcelaGerarBoletoDto(Parcela parcela)
        {
            TokenRetorno = parcela.TokenBoleto!.Value;
            IdentificadorContrato = parcela.Contrato!.Id.ToString();
            DataVencimento = parcela.DataVencimento;
            Valor = parcela.Valor;
            Cliente = new ClienteParcelaGerarBoletoDto(
                parcela.Contrato!.Cliente!.Nome,
                parcela.Contrato!.Cliente!.Cpf,
                parcela.Contrato!.Cliente!.Endereco);
        }
    }

    public record ClienteParcelaGerarBoletoDto
    {
        public string Nome = string.Empty;

        public string Cpf = string.Empty;

        public Endereco Endereco = null!;

        public ClienteParcelaGerarBoletoDto(string nome, string cpf, Endereco endereco)
        {
            Nome = nome;
            Cpf = cpf;
            Endereco = endereco;
        }
    }
}