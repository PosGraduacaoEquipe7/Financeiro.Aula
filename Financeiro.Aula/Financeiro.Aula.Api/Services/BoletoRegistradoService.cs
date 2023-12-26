using Financeiro.Aula.Domain.Interfaces.Queues;

namespace Financeiro.Aula.Api.Services
{
    public class BoletoRegistradoService : BackgroundService
    {
        private readonly IBoletoRegistradoQueueConsumer _queue;

        public BoletoRegistradoService(IServiceScopeFactory scopeFactory)
        {
            using var scope = scopeFactory.CreateScope();

            _queue = scope.ServiceProvider.GetRequiredService<IBoletoRegistradoQueueConsumer>();
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _queue.Execute(stoppingToken);

            return Task.CompletedTask;
        }

        public override void Dispose()
        {
            _queue.Close();

            base.Dispose();
        }
    }
}