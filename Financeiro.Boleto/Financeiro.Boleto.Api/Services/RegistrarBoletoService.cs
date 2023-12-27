using Financeiro.Boleto.Domain.Interfaces.Queues;

namespace Financeiro.Boleto.Api.Services
{
    public class RegistrarBoletoService : IHostedService
    {
        private readonly IRegistrarBoletoQueueConsumer _queue;

        public RegistrarBoletoService(IServiceScopeFactory scopeFactory)
        {
            using var scope = scopeFactory.CreateScope();

            _queue = scope.ServiceProvider.GetRequiredService<IRegistrarBoletoQueueConsumer>();
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