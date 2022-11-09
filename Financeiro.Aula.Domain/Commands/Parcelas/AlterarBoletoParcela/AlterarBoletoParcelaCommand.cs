using MediatR;

namespace Financeiro.Aula.Domain.Commands.Parcelas.AlterarBoletoParcela
{
    public class AlterarBoletoParcelaCommand : IRequest<(bool Sucesso, string Mensagem)>
    {
        public long ParcelaId { get; private set; }

        public string ChaveBoleto { get; private set; }

        public AlterarBoletoParcelaCommand(long parcelaId, string chaveBoleto)
        {
            ParcelaId = parcelaId;
            ChaveBoleto = chaveBoleto;
        }
    }
}