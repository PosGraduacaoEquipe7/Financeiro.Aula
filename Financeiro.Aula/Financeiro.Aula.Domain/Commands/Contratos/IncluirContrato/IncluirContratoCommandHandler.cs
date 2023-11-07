using Financeiro.Aula.Domain.Entities;
using Financeiro.Aula.Domain.Interfaces.DomainServices;
using Financeiro.Aula.Domain.Interfaces.Repositories;
using MediatR;

namespace Financeiro.Aula.Domain.Commands.Contratos.IncluirContrato
{
    public class IncluirContratoCommandHandler : IRequestHandler<IncluirContratoCommand, Contrato?>
    {
        private readonly IContratoRepository _contratoRepository;
        private readonly IParcelaRepository _parcelaRepository;
        private readonly IParcelaService _parcelaService;
        private readonly ITurmaRepository _turmaRepository;

        public IncluirContratoCommandHandler(
            IContratoRepository contratoRepository,
            IParcelaRepository parcelaRepository,
            IParcelaService parcelaService,
            ITurmaRepository turmaRepository)
        {
            _contratoRepository = contratoRepository;
            _parcelaRepository = parcelaRepository;
            _parcelaService = parcelaService;
            _turmaRepository = turmaRepository;
        }

        public async Task<Contrato?> Handle(IncluirContratoCommand request, CancellationToken cancellationToken)
        {
            var turma = await _turmaRepository.ObterTurmaDoCurso(Curso.CURSO_PADRAO_ID);
            if (turma is null)
                return default;

            var contrato = new Contrato(
                id: 0,
                dataEmissao: request.DataEmissao,
                valorTotal: request.ValorTotal,
                clienteId: request.ClienteId,
                turmaId: turma.Id);

            await _contratoRepository.IncluirContrato(contrato);

            var retorno = _parcelaService.GerarParcelas(request.ValorTotal, request.NumeroParcelas, request.DataPrimeiroVencimento, contrato.Id);

            if (retorno.Sucesso && retorno.Parcelas!.Any())
                await _parcelaRepository.IncluirParcelas(retorno.Parcelas!);

            return contrato;
        }
    }
}