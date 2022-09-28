using Financeiro.Aula.Domain.DTOs;
using Financeiro.Aula.Domain.Entities;
using Financeiro.Aula.Domain.Interfaces.DomainServices;
using Financeiro.Aula.Domain.Interfaces.Repositories;
using Financeiro.Aula.Domain.Interfaces.Services;
using MediatR;

namespace Financeiro.Aula.Domain.Commands.Matriculas.GerarContratoDaMatricula
{
    public class GerarContratoDaMatriculaCommandHandler : IRequestHandler<GerarContratoDaMatriculaCommand, Contrato?>
    {
        private readonly IAuthService _authService;
        private readonly IClienteService _clienteService;
        private readonly IContratoRepository _contratoRepository;
        private readonly IParcelaRepository _parcelaRepository;
        private readonly ICursoRepository _cursoRepository;
        private readonly IParcelaService _parcelaService;
        private readonly ITurmaRepository _turmaRepository;

        public GerarContratoDaMatriculaCommandHandler(
            IAuthService authService,
            IClienteService clienteService,
            IContratoRepository contratoRepository,
            IParcelaRepository parcelaRepository,
            ICursoRepository cursoRepository,
            IParcelaService parcelaService,
            ITurmaRepository turmaRepository)
        {
            _authService = authService;
            _clienteService = clienteService;
            _contratoRepository = contratoRepository;
            _parcelaRepository = parcelaRepository;
            _cursoRepository = cursoRepository;
            _parcelaService = parcelaService;
            _turmaRepository = turmaRepository;
        }

        public async Task<Contrato?> Handle(GerarContratoDaMatriculaCommand request, CancellationToken cancellationToken)
        {
            if (!_authService.UsuarioLogado)
                return null;

            var clienteDto = new ClienteAtualizacaoDto(
                UsuarioId: _authService.UsuarioId,
                Nome: request.Nome,
                Email: request.Email,
                Cpf: request.Cpf,
                Identidade: request.Identidade,
                DataNascimento: request.DataNascimento,
                Telefone: request.Telefone,
                Endereco: request.Endereco
            );

            var cliente = await _clienteService.IncluirOuAlterarCliente(clienteDto);

            var curso = await _cursoRepository.ObterCursoPadrao();
            if (curso is null)
                return default;

            var turma = await _turmaRepository.ObterTurmaDoCurso(curso.Id);
            if (turma is null)
                return default;

            var contrato = new Contrato(
                id: 0,
                dataEmissao: DateTime.Now,
                valorTotal: curso.ValorBruto,
                clienteId: cliente.Id,
                turmaId: turma.Id);

            await _contratoRepository.IncluirContrato(contrato);

            var retorno = _parcelaService.GerarParcelas(curso.ValorBruto, request.NumeroParcelas, DateTime.Now.Date.AddMonths(1), contrato.Id);

            if (retorno.Sucesso && retorno.Parcelas!.Any())
                await _parcelaRepository.IncluirParcelas(retorno.Parcelas!);

            return contrato;
        }
    }
}