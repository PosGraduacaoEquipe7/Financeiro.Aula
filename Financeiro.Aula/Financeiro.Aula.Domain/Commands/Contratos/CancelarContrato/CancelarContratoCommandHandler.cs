using Financeiro.Aula.Domain.Interfaces.Repositories;
using MediatR;

namespace Financeiro.Aula.Domain.Commands.Contratos.CancelarContrato
{
    public class CancelarContratoCommandHandler : IRequestHandler<CancelarContratoCommand, (bool Sucesso, string Mensagem)>
    {
        private readonly IContratoRepository _contratoRepository;

        public CancelarContratoCommandHandler(IContratoRepository contratoRepository)
        {
            _contratoRepository = contratoRepository;
        }

        public async Task<(bool Sucesso, string Mensagem)> Handle(CancelarContratoCommand request, CancellationToken cancellationToken)
        {
            var contrato = await _contratoRepository.ObterContrato(request.Id);

            if (contrato is null)
                return (false, "Contrato não encontrado");

            if (contrato.Cancelado)
                return (false, "Contrato já cancelado");

            contrato.Cancelar();

            await _contratoRepository.AlterarContrato(contrato);

            return (true, String.Empty);
        }
    }
}