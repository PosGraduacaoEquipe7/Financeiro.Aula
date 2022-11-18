using Financeiro.Aula.Domain.Entities;
using MediatR;

namespace Financeiro.Aula.Domain.Commands.Clientes.ListarClientes
{
    public class ListarClientesCommand : IRequest<IEnumerable<Cliente>>
    {
        public string? Nome { get; private set; }

        public ListarClientesCommand(string? nome)
        {
            Nome = nome;
        }
    }
}