using Financeiro.Aula.Domain.Entities;
using MediatR;

namespace Financeiro.Aula.Domain.Commands.Parcelas.ListarParcelas
{
    public class ListarParcelasCommand : IRequest<IEnumerable<Parcela>>
    {
    }
}