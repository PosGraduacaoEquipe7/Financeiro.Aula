using MediatR;

namespace Financeiro.Aula.Domain.Commands.Parcelas.GerarBoletoParcela
{
    public class GerarBoletoParcelaCommand : IRequest<(bool Sucesso, string Mensagem, byte[]? Pdf)>
    {
        public long ParcelaId { get; private set; }

        public bool ConfirmaSobrescrever { get; private set; }

        public GerarBoletoParcelaCommand(long parcelaId, bool confirmaSobrescrever)
        {
            ParcelaId = parcelaId;
            ConfirmaSobrescrever = confirmaSobrescrever;
        }
    }
}