using Financeiro.Aula.Domain.DTOs;

namespace Financeiro.Aula.Domain.Interfaces.Queues
{
    public interface IBoletoQueue
    {
        Task EnviarParcelaFilaGerarBoleto(ParcelaGerarBoletoDto parcelaDto);
    }
}