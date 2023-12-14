using Financeiro.Aula.Domain.Interfaces.Queues;

namespace Financeiro.Aula.Api.Services
{
    public class BoletoRegistradoService : BackgroundService
    {
        private readonly IServiceScopeFactory _scopeFactory;

        public BoletoRegistradoService(IServiceScopeFactory scopeFactory)
        {
            _scopeFactory = scopeFactory;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            using var scope = _scopeFactory.CreateScope();

            var boletoRegistradoQueue = scope.ServiceProvider.GetRequiredService<IBoletoRegistradoQueueConsumer>();

            boletoRegistradoQueue.Execute(stoppingToken);

            return Task.CompletedTask;
        }
    }
}