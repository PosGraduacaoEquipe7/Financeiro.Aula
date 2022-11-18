namespace Financeiro.Boleto.Queue.GerarBoleto.Scopes
{
    public interface IScopedProcessingService
    {
        Task DoWorkAsync(CancellationToken stoppingToken);
    }
}