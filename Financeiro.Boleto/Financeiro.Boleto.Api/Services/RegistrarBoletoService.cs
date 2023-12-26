using Financeiro.Boleto.Domain.Interfaces.Queues;

namespace Financeiro.Boleto.Api.Services
{
    public class RegistrarBoletoService : BackgroundService
    {
        private readonly IRegistrarBoletoQueueConsumer _queue;

        public RegistrarBoletoService(IServiceScopeFactory scopeFactory)
        {
            using var scope = scopeFactory.CreateScope();

            _queue = scope.ServiceProvider.GetRequiredService<IRegistrarBoletoQueueConsumer>();
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