using Financeiro.Aula.Domain.Entities;
using Financeiro.Aula.Domain.Interfaces.Repositories;
using MediatR;

namespace Financeiro.Aula.Domain.Commands.Clientes.ListarClientes
{
    public class ListarClientesCommandHandler : IRequestHandler<ListarClientesCommand, IEnumerable<Cliente>>
    {
        private readonly IClienteRepository _clienteRepository;

        public ListarClientesCommandHandler(IClienteRepository clienteRepository)
        {
            _clienteRepository = clienteRepository;
        }

        public async Task<IEnumerable<Cliente>> Handle(ListarClientesCommand request, CancellationToken cancellationToken)
        {
            return await _clienteRepository.ListarClientes(request.Nome);
        }
    }
}