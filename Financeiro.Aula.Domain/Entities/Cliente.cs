using Financeiro.Aula.Domain.ValueObjects;

namespace Financeiro.Aula.Domain.Entities
{
    public class Cliente
    {
        public long Id { get; private set; }

        public string Nome { get; private set; }

        public string Cpf { get; private set; }

        public string Identidade { get; private set; }

        public DateTime DataNascimento { get; private set; }

        public string Telefone { get; private set; }

        public Endereco Endereco { get; private set; }

        public virtual ICollection<Contrato> Contratos { get; private set; }

        private Cliente() : this(default, string.Empty, string.Empty, string.Empty, DateTime.Now, string.Empty, null!)
        {
        }

        public Cliente(long id, string nome, string cpf, string identidade, DateTime dataNascimento, string telefone, Endereco endereco)
        {
            Id = id;
            Nome = nome;
            Cpf = cpf;
            Identidade = identidade;
            DataNascimento = dataNascimento;
            Telefone = telefone;
            Endereco = endereco;

            Contratos = new HashSet<Contrato>();
        }

        public void AtualizarCadastro(string nome, string cpf, Endereco endereco)
        {
            Nome = nome;
            Cpf = cpf;
            Endereco = endereco;
        }
    }
}