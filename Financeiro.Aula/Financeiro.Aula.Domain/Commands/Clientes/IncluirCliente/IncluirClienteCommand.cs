using Financeiro.Aula.Domain.Entities;
using Financeiro.Aula.Domain.ValueObjects;
using MediatR;

namespace Financeiro.Aula.Domain.Commands.Clientes.IncluirCliente
{
    public class IncluirClienteCommand : IRequest<Cliente>
    {
        public string Nome { get; set; }
        public string Email { get; set; }
        public string Cpf { get; set; }
        public string Identidade { get; set; }
        public DateTime DataNascimento { get; set; }
        public string Telefone { get; set; }
        public Endereco Endereco { get; set; }
    }
}