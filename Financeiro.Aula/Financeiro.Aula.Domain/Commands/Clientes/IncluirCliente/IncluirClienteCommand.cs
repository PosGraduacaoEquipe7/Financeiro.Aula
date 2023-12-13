using Financeiro.Aula.Domain.Entities;
using Financeiro.Aula.Domain.ValueObjects;
using MediatR;

namespace Financeiro.Aula.Domain.Commands.Clientes.IncluirCliente
{
    public class IncluirClienteCommand : IRequest<Cliente?>
    {
        public string Nome { get; private set; }
        public string Email { get; private set; }
        public string Cpf { get; private set; }
        public string Identidade { get; private set; }
        public DateTime DataNascimento { get; private set; }
        public string Telefone { get; private set; }
        public Endereco Endereco { get; private set; }

        //public IncluirClienteCommand() : this(string.Empty, string.Empty, string.Empty, string.Empty, DateTime.Now.Date, string.Empty, new())
        //{
        //}

        public IncluirClienteCommand(string nome, string email, string cpf, string identidade, DateTime dataNascimento, string telefone, Endereco endereco)
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