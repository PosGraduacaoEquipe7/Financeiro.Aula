using MediatR;

namespace Financeiro.Aula.Domain.Commands.Parcelas.ObterPdfBoletoParcela
{
    public class ObterPdfBoletoParcelaCommand : IRequest<(bool Sucesso, string Mensagem, string Pdf)>
    {
        public long ParcelaId { get; private set; }

        public ObterPdfBoletoParcelaCommand(long parcelaId)
        {
            ParcelaId = parcelaId;
        }
    }
}