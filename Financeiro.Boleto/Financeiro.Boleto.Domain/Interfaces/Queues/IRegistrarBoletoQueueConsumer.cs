namespace Financeiro.Boleto.Domain.Interfaces.Queues
{
    public interface IRegistrarBoletoQueueConsumer
    {
        Task Execute(CancellationToken cancellationToken);
    }
}