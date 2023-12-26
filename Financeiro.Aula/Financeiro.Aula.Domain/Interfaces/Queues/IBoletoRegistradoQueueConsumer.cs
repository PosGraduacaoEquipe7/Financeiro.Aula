namespace Financeiro.Aula.Domain.Interfaces.Queues
{
    public interface IBoletoRegistradoQueueConsumer
    {
        Task Execute(CancellationToken cancellationToken);
        void Close();
    }
}