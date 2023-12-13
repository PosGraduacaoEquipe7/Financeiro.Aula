using Financeiro.Aula.Domain.Entities;
using Financeiro.Aula.Domain.Interfaces.Repositories;
using MediatR;

namespace Financeiro.Aula.Domain.Commands.Clientes.ListarClientes
{
    public class ListarClientesCommandHandler : IRequestHandler<ListarClientesCommand, IEnumerable<ListarClientesCommandResult>>
    {
        private readonly IClienteRepository _clienteRepository;

        public ListarClientesCommandHandler(IClienteRepository clienteRepository)
        {
            _clienteRepository = clienteRepository;
        }

        public async Task<IEnumerable<ListarClientesCommandResult>> Handle(ListarClientesCommand request, CancellationToken cancellationToken)
        {
            var clientes = await _clienteRepository.ListarClientes(request.Nome);

            return clientes.Select(MapClienteResult);
        }

        private ListarClientesCommandResult MapClienteResult(Cliente cliente)
            => new ListarClientesCommandResult(
                cliente.Id,
                cliente.Nome,
                cliente.Email);
    }
}