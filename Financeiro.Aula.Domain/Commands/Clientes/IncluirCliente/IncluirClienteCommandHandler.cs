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
            var cliente = new Cliente(
                id: 0,
                usuarioId: Guid.NewGuid().ToString(), // TODO: esse fluxo não deveria nem existir...
                nome: request.Nome,
                email: request.Email,
                cpf: request.Cpf,
                identidade: request.Identidade,
                dataNascimento: request.DataNascimento,
                telefone: request.Telefone,
                endereco: request.Endereco);

            await _clienteRepository.IncluirCliente(cliente);

            return cliente;
        }
    }
}