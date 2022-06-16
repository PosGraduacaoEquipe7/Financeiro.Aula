using Financeiro.Aula.Domain.ValueObjects;

namespace Financeiro.Aula.Domain.Entities
{
    public class ParametroBoleto
    {
        public long Id { get; private set; }

        public string Descricao { get; private set; }

        public string Banco { get; private set; }

        public string Agencia { get; private set; }

        public string NumeroConta { get; private set; }

        public string Carteira { get; private set; }

        public int NumeroBoletoAtual { get; private set; }

        public string NomeBeneficiario { get; private set; }

        public string CnpjBeneficiario { get; private set; }

        public Endereco EnderecoBeneficiario { get; private set; }

        private ParametroBoleto()
        {
        }

        public ParametroBoleto(long id, string descricao, string banco, string agencia, string numeroConta, string carteira, int numeroBoletoAtual, string nomeBeneficiario, string cnpjBeneficiario, Endereco enderecoBeneficiario)
        {
            Id = id;
            Descricao = descricao;
            Banco = banco;
            Agencia = agencia;
            NumeroConta = numeroConta;
            Carteira = carteira;
            NumeroBoletoAtual = numeroBoletoAtual;
            NomeBeneficiario = nomeBeneficiario;
            CnpjBeneficiario = cnpjBeneficiario;
            EnderecoBeneficiario = enderecoBeneficiario;
        }

        public string ObterProximoNumeroFormatado()
        {
            return $"{(++NumeroBoletoAtual).ToString().PadLeft(11, '0')}-P";
        }
    }
}