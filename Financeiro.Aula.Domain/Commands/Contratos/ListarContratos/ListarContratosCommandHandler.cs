using Financeiro.Aula.Domain.Entities;
using Financeiro.Aula.Domain.Interfaces.Repositories;
using MediatR;

namespace Financeiro.Aula.Domain.Commands.Contratos.ListarContratos
{
    public class ListarContratosCommandHandler : IRequestHandler<ListarContratosCommand, IEnumerable<Contrato>>
    {
        private readonly IContratoRepository _contratoRepository;

        public ListarContratosCommandHandler(IContratoRepository contratoRepository)
        {
            _contratoRepository = contratoRepository;
        }

        public async Task<IEnumerable<Contrato>> Handle(ListarContratosCommand request, CancellationToken cancellationToken)
        {
            return await _contratoRepository.ListarContratos(request.ClienteId);
        }
    }
}