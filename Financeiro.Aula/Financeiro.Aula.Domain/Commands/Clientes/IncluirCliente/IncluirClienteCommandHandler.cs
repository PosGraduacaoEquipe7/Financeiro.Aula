using Financeiro.Aula.Domain.Entities;
using Financeiro.Aula.Domain.Interfaces.Repositories;
using Financeiro.Aula.Domain.Interfaces.Services;
using MediatR;

namespace Financeiro.Aula.Domain.Commands.Clientes.IncluirCliente
{
    public class IncluirClienteCommandHandler : IRequestHandler<IncluirClienteCommand, Cliente>
    {
        private readonly IClienteRepository _clienteRepository;
        private readonly IAuthService _authService;

        public IncluirClienteCommandHandler(IClienteRepository clienteRepository, IAuthService authService)
        {
            _clienteRepository = clienteRepository;
            _authService = authService;
        }

        public async Task<Cliente> Handle(IncluirClienteCommand request, CancellationToken cancellationToken)
        {
            var cliente = new Cliente(
                id: 0,
                usuarioId: _authService.UsuarioId,
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