using MediatR;

namespace Financeiro.Aula.Domain.Commands.Parcelas.PagarParcela
{
    public class PagarParcelaCommand : IRequest<(bool Sucesso, string Mensagem)>
    {
        public long Id { get; private set; }

        public PagarParcelaCommand(long id)
        {
            Id = id;
        }
    }
}