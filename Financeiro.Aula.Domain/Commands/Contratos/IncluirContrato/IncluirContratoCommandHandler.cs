using Financeiro.Aula.Domain.Entities;
using Financeiro.Aula.Domain.Interfaces.DomainServices;
using Financeiro.Aula.Domain.Interfaces.Repositories;
using MediatR;

namespace Financeiro.Aula.Domain.Commands.Contratos.IncluirContrato
{
    public class IncluirContratoCommandHandler : IRequestHandler<IncluirContratoCommand, Contrato>
    {
        private readonly IContratoRepository _contratoRepository;
        private readonly IParcelaRepository _parcelaRepository;
        private readonly IParcelaService _parcelaService;

        public IncluirContratoCommandHandler(IContratoRepository contratoRepository, IParcelaRepository parcelaRepository, IParcelaService parcelaService)
        {
            _contratoRepository = contratoRepository;
            _parcelaRepository = parcelaRepository;
            _parcelaService = parcelaService;
        }

        public async Task<Contrato> Handle(IncluirContratoCommand request, CancellationToken cancellationToken)
        {
            var contrato = new Contrato(id: 0, dataEmissao: request.DataEmissao, valorTotal: request.ValorTotal, clienteId: request.ClienteId);

            await _contratoRepository.IncluirContrato(contrato);

            var retorno = _parcelaService.GerarParcelas(request.ValorTotal, request.NumeroParcelas, request.DataPrimeiroVencimento, contrato.Id);

            if (retorno.Sucesso && retorno.Parcelas!.Any())
                await _parcelaRepository.IncluirParcelas(retorno.Parcelas!);

            return contrato;
        }
    }
}