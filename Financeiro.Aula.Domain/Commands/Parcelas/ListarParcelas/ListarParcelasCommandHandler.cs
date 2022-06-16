using Financeiro.Aula.Domain.Entities;
using Financeiro.Aula.Domain.Interfaces.Repositories;
using MediatR;

namespace Financeiro.Aula.Domain.Commands.Parcelas.ListarParcelas
{
    public class ListarParcelasCommandHandler : IRequestHandler<ListarParcelasCommand, IEnumerable<Parcela>>
    {
        private readonly IParcelaRepository _parcelaRepository;

        public ListarParcelasCommandHandler(IParcelaRepository parcelaRepository)
        {
            _parcelaRepository = parcelaRepository;
        }

        public Task<IEnumerable<Parcela>> Handle(ListarParcelasCommand request, CancellationToken cancellationToken)
        {
            return _parcelaRepository.ListarParcelas(request.ContratoId);
        }
    }
}