using Financeiro.Aula.Domain.Interfaces.Queues;

namespace Financeiro.Aula.Api.Services
{
    public class BoletoRegistradoService : IHostedService
    {
        private readonly IBoletoRegistradoQueueConsumer _queue;

        public BoletoRegistradoService(IServiceScopeFactory scopeFactory)
        {
            using var scope = scopeFactory.CreateScope();

            _queue = scope.ServiceProvider.GetRequiredService<IBoletoRegistradoQueueConsumer>();
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _queue.Execute(cancellationToken);

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _queue.Close();

            return Task.CompletedTask;
        }
    }
}