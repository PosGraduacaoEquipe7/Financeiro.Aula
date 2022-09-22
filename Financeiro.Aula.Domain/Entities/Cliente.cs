using Financeiro.Aula.Domain.ValueObjects;

namespace Financeiro.Aula.Domain.Entities
{
    public class Cliente
    {
        public long Id { get; private set; }

        public string UsuarioId { get; private set; }

        public string Nome { get; private set; }

        public string Email { get; private set; }

        public string Cpf { get; private set; }

        public string Identidade { get; private set; }

        public DateTime DataNascimento { get; private set; }

        public string Telefone { get; private set; }

        public Endereco Endereco { get; private set; }

        public virtual ICollection<Contrato> Contratos { get; private set; }

        private Cliente() : this(default, Guid.NewGuid().ToString(), string.Empty, string.Empty, string.Empty, string.Empty, DateTime.Now, string.Empty, null!)
        {
        }

        public Cliente(string usuarioId, string nome, string email, string cpf, string identidade, DateTime dataNascimento, string telefone, Endereco endereco)
        {
            UsuarioId = usuarioId;
            Nome = nome;
            Email = email;
            Cpf = cpf;
            Identidade = identidade;
            DataNascimento = dataNascimento;
            Telefone = telefone;
            Endereco = endereco;

            Contratos = new HashSet<Contrato>();
        }

        public Cliente(long id, string usuarioId, string nome, string email, string cpf, string identidade, DateTime dataNascimento, string telefone, Endereco endereco)
            : this(usuarioId, nome, email, cpf, identidade, dataNascimento, telefone, endereco)
        {
            Id = id;
        }

        public void AtualizarCadastro(string nome, string email, string cpf, string identidade, DateTime dataNascimento, string telefone, Endereco endereco)
        {
            Nome = nome;
            Email = email;
            Cpf = cpf;
            Identidade = identidade;
            DataNascimento = dataNascimento;
            Telefone = telefone;
            Endereco = endereco;
        }
    }
}