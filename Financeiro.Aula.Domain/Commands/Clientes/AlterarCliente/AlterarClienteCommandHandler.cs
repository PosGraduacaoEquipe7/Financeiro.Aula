using Financeiro.Aula.Domain.Entities;
using Financeiro.Aula.Domain.Interfaces.Repositories;
using MediatR;

namespace Financeiro.Aula.Domain.Commands.Clientes.AlterarCliente
{
    public class AlterarClienteCommandHandler : IRequestHandler<AlterarClienteCommand, Cliente?>
    {
        private readonly IClienteRepository _clienteRepository;

        public AlterarClienteCommandHandler(IClienteRepository clienteRepository)
        {
            _clienteRepository = clienteRepository;
        }

        public async Task<Cliente?> Handle(AlterarClienteCommand request, CancellationToken cancellationToken)
        {
            var cliente = await _clienteRepository.ObterCliente(request.Id);

            if (cliente is null)
                return default;

            cliente.AtualizarCadastro(
                request.Nome,
                request.Email,
                request.Cpf,
                request.Identidade,
                request.DataNascimento,
                request.Telefone,
                request.Endereco);

            await _clienteRepository.AtualizarCliente(cliente);

            return cliente;
        }
    }
}