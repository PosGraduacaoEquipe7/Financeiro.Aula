using Financeiro.Aula.Domain.Entities;
using Financeiro.Aula.Domain.ValueObjects;
using MediatR;

namespace Financeiro.Aula.Domain.Commands.Clientes.AlterarCliente
{
    public class AlterarClienteCommand : IRequest<Cliente?>
    {
        public long Id { get; private set; }
        public string Nome { get; set; }
        public string Email { get; set; }
        public string Cpf { get; set; }
        public string Identidade { get; set; }
        public DateTime DataNascimento { get; set; }
        public string Telefone { get; set; }
        public Endereco Endereco { get; set; }

        public AlterarClienteCommand AgregarId(long id)
        {
            Id = id;
            return this;
        }
    }
}