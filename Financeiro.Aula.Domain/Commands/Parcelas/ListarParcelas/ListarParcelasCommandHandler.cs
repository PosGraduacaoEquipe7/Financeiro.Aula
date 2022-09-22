using Financeiro.Aula.Domain.Entities;
using Financeiro.Aula.Domain.Interfaces.Repositories;
using Financeiro.Aula.Domain.Interfaces.Services;
using MediatR;

namespace Financeiro.Aula.Domain.Commands.Parcelas.ListarParcelas
{
    public class ListarParcelasCommandHandler : IRequestHandler<ListarParcelasCommand, IEnumerable<Parcela>>
    {
        private readonly IParcelaRepository _parcelaRepository;
        private readonly IAuthService _authService;

        public ListarParcelasCommandHandler(IParcelaRepository parcelaRepository, IAuthService authService)
        {
            _parcelaRepository = parcelaRepository;
            _authService = authService;
        }

        public Task<IEnumerable<Parcela>> Handle(ListarParcelasCommand request, CancellationToken cancellationToken)
        {
            var logado = _authService.UsuarioLogado;
            var usuario = _authService.UsuarioId;

            return _parcelaRepository.ListarParcelas(request.ContratoId);
        }
    }
}