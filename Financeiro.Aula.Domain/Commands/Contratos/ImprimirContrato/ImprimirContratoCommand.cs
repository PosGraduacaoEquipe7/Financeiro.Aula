using MediatR;

namespace Financeiro.Aula.Domain.Commands.Contratos.ImprimirContrato
{
    public class ImprimirContratoCommand : IRequest<(bool Sucesso, string Mensagem, string? Contrato)>
    {
        public long Id { get; private set; }

        public ImprimirContratoCommand(long id)
        {
            Id = id;
        }
    }
}