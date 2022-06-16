using Financeiro.Aula.Domain.ValueObjects;

namespace Financeiro.Aula.Domain.Entities
{
    public class Cliente
    {
        public long Id { get; private set; }

        public string Nome { get; private set; }

        public string Cpf { get; private set; }

        public Endereco Endereco { get; private set; }

        public virtual ICollection<Contrato> Contratos { get; private set; }

        private Cliente()
        {
        }

        public Cliente(long id, string nome, string cpf, Endereco endereco)
        {
            Id = id;
            Nome = nome;
            Cpf = cpf;
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