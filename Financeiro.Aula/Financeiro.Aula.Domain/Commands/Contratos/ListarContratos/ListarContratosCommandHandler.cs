using Financeiro.Aula.Domain.Entities;
using Financeiro.Aula.Domain.Interfaces.Repositories;
using Financeiro.Aula.Domain.Interfaces.Services;
using MediatR;

namespace Financeiro.Aula.Domain.Commands.Contratos.ListarContratos
{
    public class ListarContratosCommandHandler : IRequestHandler<ListarContratosCommand, IEnumerable<Contrato>>
    {
        private readonly IContratoRepository _contratoRepository;
        private readonly IAuthService _authService;

        public ListarContratosCommandHandler(IContratoRepository contratoRepository, IAuthService authService)
        {
            _contratoRepository = contratoRepository;
            _authService = authService;
        }

        public async Task<IEnumerable<Contrato>> Handle(ListarContratosCommand request, CancellationToken cancellationToken)
        {
            return await _contratoRepository.ListarContratosPeloUsuario(_authService.UsuarioId);
        }
    }
}