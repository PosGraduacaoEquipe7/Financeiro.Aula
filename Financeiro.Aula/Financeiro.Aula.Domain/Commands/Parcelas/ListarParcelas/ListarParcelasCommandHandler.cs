using Financeiro.Aula.Domain.Entities;
using Financeiro.Aula.Domain.Interfaces.Repositories;
using Financeiro.Aula.Domain.Interfaces.Services;
using MediatR;

namespace Financeiro.Aula.Domain.Commands.Parcelas.ListarParcelas
{
    public class ListarParcelasCommandHandler : IRequestHandler<ListarParcelasCommand, IEnumerable<Parcela>>
    {
        private readonly IParcelaRepository _parcelaRepository;
        private readonly IContratoRepository _contratoRepository;
        private readonly IAuthService _authService;

        public ListarParcelasCommandHandler(IParcelaRepository parcelaRepository, IContratoRepository contratoRepository, IAuthService authService)
        {
            _parcelaRepository = parcelaRepository;
            _contratoRepository = contratoRepository;
            _authService = authService;
        }

        public async Task<IEnumerable<Parcela>> Handle(ListarParcelasCommand request, CancellationToken cancellationToken)
        {
            var logado = _authService.UsuarioLogado;
            var usuario = _authService.UsuarioId;

            if (!logado)
                return Enumerable.Empty<Parcela>();

            var contrato = await _contratoRepository.ObterContrato(request.ContratoId!.Value);

            if (contrato?.Cliente?.UsuarioId != usuario)
                return Enumerable.Empty<Parcela>();

            return await _parcelaRepository.ListarParcelas(request.ContratoId);
        }
    }
}