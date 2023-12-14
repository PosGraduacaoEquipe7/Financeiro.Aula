using Financeiro.Boleto.Domain.DTOs;

namespace Financeiro.Boleto.Domain.Interfaces.Queues
{
    public interface IBoletoRegistradoQueue
    {
        Task EnviarFilaBoletoRegistrado(BoletoRegistradoDto boletoDto);
    }
}