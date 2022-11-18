using Financeiro.Aula.Domain.Interfaces.Repositories;
using Financeiro.Aula.Domain.Interfaces.Services;
using MediatR;

namespace Financeiro.Aula.Domain.Commands.Contratos.AceitarContrato
{
    public class AceitarContratoCommandHandler : IRequestHandler<AceitarContratoCommand, (bool Sucesso, string Mensagem)>
    {
        private readonly IAuthService _authService;
        private readonly IContratoRepository _contratoRepository;

        public AceitarContratoCommandHandler(IAuthService authService, IContratoRepository contratoRepository)
        {
            _authService = authService;
            _contratoRepository = contratoRepository;
        }

        public async Task<(bool Sucesso, string Mensagem)> Handle(AceitarContratoCommand request, CancellationToken cancellationToken)
        {
            var contrato = await _contratoRepository.ObterContrato(request.Id);

            if (contrato is null)
                return (false, "Contrato não encontrado");

            if (contrato.Cliente?.UsuarioId != _authService.UsuarioId)
                return (false, "O contrato é inválido");

            if (contrato.Cancelado)
                return (false, "O contrato está cancelado");

            if (contrato.ContratoAceite)
                return (false, "O contrato já teve seu aceite confirmado");
                
            contrato.AceitarContrato();

            await _contratoRepository.AlterarContrato(contrato);

            return (true, string.Empty);
        }
    }
}