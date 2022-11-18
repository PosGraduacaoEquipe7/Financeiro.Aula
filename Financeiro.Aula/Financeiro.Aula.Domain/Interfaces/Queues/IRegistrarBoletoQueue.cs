using Financeiro.Aula.Domain.DTOs;

namespace Financeiro.Aula.Domain.Interfaces.Queues
{
    public interface IRegistrarBoletoQueue
    {
        Task EnviarParcelaFilaGerarBoleto(ParcelaGerarBoletoDto parcelaDto);
    }
}