namespace Financeiro.Aula.Queue.BoletoRegistrado.Scopes
{
    public interface IScopedProcessingService
    {
        Task DoWorkAsync(CancellationToken stoppingToken);
    }
}