using MediatR;

namespace Financeiro.Aula.Domain.Commands.Contratos.CancelarContrato
{
    public class CancelarContratoCommand : IRequest<(bool Sucesso, string Mensagem)>
    {
        public long Id { get; private set; }

        public CancelarContratoCommand(long id)
        {
            Id = id;
        }
    }
}