using Financeiro.Aula.Domain.Entities;
using Financeiro.Aula.Domain.Interfaces.Repositories;
using MediatR;

namespace Financeiro.Aula.Domain.Commands.Clientes.IncluirCliente
{
    public class IncluirClienteCommandHandler : IRequestHandler<IncluirClienteCommand, Cliente>
    {
        private readonly IClienteRepository _clienteRepository;

        public IncluirClienteCommandHandler(IClienteRepository clienteRepository)
        {
            _clienteRepository = clienteRepository;
        }

        public async Task<Cliente> Handle(IncluirClienteCommand request, CancellationToken cancellationToken)
        {
            var cliente = new Cliente(id: 0, nome: request.Nome, cpf: request.Cpf, endereco: request.Endereco);

            await _clienteRepository.IncluirCliente(cliente);

            return cliente;
        }
    }
}