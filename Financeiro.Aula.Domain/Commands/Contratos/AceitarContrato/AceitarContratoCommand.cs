using MediatR;

namespace Financeiro.Aula.Domain.Commands.Contratos.AceitarContrato
{
    public class AceitarContratoCommand : IRequest<(bool Sucesso, string Mensagem)>
    {
        public long Id { get; private set; }

        public AceitarContratoCommand(long id)
        {
            Id = id;
        }
    }
}