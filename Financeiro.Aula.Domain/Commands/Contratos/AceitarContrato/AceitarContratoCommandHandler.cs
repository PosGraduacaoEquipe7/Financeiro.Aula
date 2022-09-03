using Financeiro.Aula.Domain.Interfaces.Repositories;
using MediatR;

namespace Financeiro.Aula.Domain.Commands.Contratos.AceitarContrato
{
    public class AceitarContratoCommandHandler : IRequestHandler<AceitarContratoCommand, (bool Sucesso, string Mensagem)>
    {
        private readonly IContratoRepository _contratoRepository;

        public AceitarContratoCommandHandler(IContratoRepository contratoRepository)
        {
            _contratoRepository = contratoRepository;
        }

        public async Task<(bool Sucesso, string Mensagem)> Handle(AceitarContratoCommand request, CancellationToken cancellationToken)
        {
            var contrato = await _contratoRepository.ObterContrato(request.Id);

            if (contrato is null)
                return (false, "Contrato não encontrado");

            // TODO: validar se o contrato pertence ao usuário logado

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