using Financeiro.Boleto.Domain.DTOs;

namespace Financeiro.Boleto.Domain.Services.Queues
{
    public interface IBoletoRegistradoQueue
    {
        Task EnviarFilaBoletoRegistrado(BoletoRegistradoDto boletoDto);
    }
}