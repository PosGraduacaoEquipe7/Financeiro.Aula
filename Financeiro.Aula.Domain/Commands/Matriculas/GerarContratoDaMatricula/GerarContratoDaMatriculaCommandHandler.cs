using Financeiro.Aula.Domain.Entities;
using Financeiro.Aula.Domain.Interfaces.DomainServices;
using Financeiro.Aula.Domain.Interfaces.Repositories;
using MediatR;

namespace Financeiro.Aula.Domain.Commands.Matriculas.GerarContratoDaMatricula
{
    public class GerarContratoDaMatriculaCommandHandler : IRequestHandler<GerarContratoDaMatriculaCommand, Contrato?>
    {
        private readonly IClienteRepository _clienteRepository;
        private readonly IContratoRepository _contratoRepository;
        private readonly IParcelaRepository _parcelaRepository;
        private readonly ICursoRepository _cursoRepository;
        private readonly IParcelaService _parcelaService;

        public GerarContratoDaMatriculaCommandHandler(
            IClienteRepository clienteRepository,
            IContratoRepository contratoRepository,
            IParcelaRepository parcelaRepository,
            ICursoRepository cursoRepository,
            IParcelaService parcelaService)
        {
            _clienteRepository = clienteRepository;
            _contratoRepository = contratoRepository;
            _parcelaRepository = parcelaRepository;
            _cursoRepository = cursoRepository;
            _parcelaService = parcelaService;
        }

        public async Task<Contrato?> Handle(GerarContratoDaMatriculaCommand request, CancellationToken cancellationToken)
        {
            if (await _clienteRepository.VerificarCpfExiste(request.Cpf))
            {
                // TODO: cpf já existente                
                return default;
            }

            // TODO: salvar o e-mail e senha

            var cliente = new Cliente(
                id: 0,
                nome: request.Nome,
                cpf: request.Cpf,
                identidade: request.Identidade,
                dataNascimento: request.DataNascimento,
                telefone: request.Telefone,
                endereco: request.Endereco);

            await _clienteRepository.IncluirCliente(cliente);

            var curso = await _cursoRepository.ObterCursoPadrao();

            if (curso is null)
            {
                // TODO: curso inválido
                return default;
            }

            // TODO: setar a turma para o contrato

            var contrato = new Contrato(
                id: 0,
                dataEmissao: DateTime.Now,
                valorTotal: curso.ValorBruto,
                clienteId: cliente.Id);

            await _contratoRepository.IncluirContrato(contrato);

            var retorno = _parcelaService.GerarParcelas(curso.ValorBruto, request.NumeroParcelas, DateTime.Now.Date, contrato.Id);

            if (retorno.Sucesso && retorno.Parcelas!.Any())
                await _parcelaRepository.IncluirParcelas(retorno.Parcelas!);

            return contrato;
        }
    }
}