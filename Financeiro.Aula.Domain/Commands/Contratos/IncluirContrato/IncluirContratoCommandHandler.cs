using Financeiro.Aula.Domain.Entities;
using Financeiro.Aula.Domain.Interfaces.Repositories;
using MediatR;

namespace Financeiro.Aula.Domain.Commands.Contratos.IncluirContrato
{
    public class IncluirContratoCommandHandler : IRequestHandler<IncluirContratoCommand, Contrato>
    {
        private readonly IContratoRepository _contratoRepository;
        private readonly IParcelaRepository _parcelaRepository;

        public IncluirContratoCommandHandler(IContratoRepository contratoRepository, IParcelaRepository parcelaRepository)
        {
            _contratoRepository = contratoRepository;
            _parcelaRepository = parcelaRepository;
        }

        public async Task<Contrato> Handle(IncluirContratoCommand request, CancellationToken cancellationToken)
        {
            var contrato = new Contrato(id: 0, dataEmissao: request.DataEmissao, valorTotal: request.ValorTotal, clienteId: request.ClienteId);

            await _contratoRepository.IncluirContrato(contrato);

            for (int i = 1; i <= request.NumeroParcelas; i++)
            {
                var parcela = new Parcela(
                    id: 0,
                    sequencial: i,
                    valor: request.ValorTotal / request.NumeroParcelas,
                    dataVencimento: request.DataPrimeiroVencimento.AddMonths(i - 1),
                    contratoId: contrato.Id);

                await _parcelaRepository.IncluirParcela(parcela);
            }

            return contrato;
        }
    }
}