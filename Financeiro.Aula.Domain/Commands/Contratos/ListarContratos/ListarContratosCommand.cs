using Financeiro.Aula.Domain.Entities;
using MediatR;

namespace Financeiro.Aula.Domain.Commands.Contratos.ListarContratos
{
    public class ListarContratosCommand : IRequest<IEnumerable<Contrato>>
    {
        public long? ClienteId { get; private set; }

        public ListarContratosCommand(long? clienteId)
        {
            ClienteId = clienteId;
        }
    }
}