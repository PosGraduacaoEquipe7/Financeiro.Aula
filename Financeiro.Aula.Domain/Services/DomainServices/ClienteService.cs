using Financeiro.Aula.Domain.DTOs;
using Financeiro.Aula.Domain.Entities;
using Financeiro.Aula.Domain.Interfaces.DomainServices;
using Financeiro.Aula.Domain.Interfaces.Repositories;

namespace Financeiro.Aula.Domain.Services.DomainServices
{
    public class ClienteService : IClienteService
    {
        private readonly IClienteRepository _clienteRepository;

        public ClienteService(IClienteRepository clienteRepository)
        {
            _clienteRepository = clienteRepository;
        }

        public async Task<Cliente> IncluirOuAlterarCliente(ClienteAtualizacaoDto clienteDto)
        {
            var cliente = await _clienteRepository.ObterClientePeloUsuarioId(clienteDto.UsuarioId);

            if (cliente is null)
            {
                cliente = new Cliente(
                    usuarioId: clienteDto.UsuarioId,
                    nome: clienteDto.Nome,
                    email: clienteDto.Email,
                    cpf: clienteDto.Cpf,
                    identidade: clienteDto.Identidade,
                    dataNascimento: clienteDto.DataNascimento,
                    telefone: clienteDto.Telefone,
                    endereco: clienteDto.Endereco
                );

                await _clienteRepository.IncluirCliente(cliente);
            }
            else
            {
                cliente.AtualizarCadastro(
                    clienteDto.Nome,
                    clienteDto.Email,
                    clienteDto.Cpf,
                    clienteDto.Identidade,
                    clienteDto.DataNascimento,
                    clienteDto.Telefone,
                    clienteDto.Endereco
                );

                await _clienteRepository.AtualizarCliente(cliente);
            }

            return cliente;
        }
    }
}