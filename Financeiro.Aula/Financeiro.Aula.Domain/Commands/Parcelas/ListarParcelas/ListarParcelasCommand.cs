using Financeiro.Aula.Domain.Entities;
using MediatR;

namespace Financeiro.Aula.Domain.Commands.Parcelas.ListarParcelas
{
    public class ListarParcelasCommand : IRequest<IEnumerable<Parcela>>
    {
        public long? ContratoId { get; private set; }

        public ListarParcelasCommand(long? contratoId)
        {
            ContratoId = contratoId;
        }
    }
}