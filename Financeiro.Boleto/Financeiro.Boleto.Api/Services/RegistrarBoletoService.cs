using Financeiro.Boleto.Domain.Interfaces.Queues;

namespace Financeiro.Boleto.Api.Services
{
    public class RegistrarBoletoService : BackgroundService
    {
        private readonly IServiceScopeFactory _scopeFactory;

        public RegistrarBoletoService(IServiceScopeFactory scopeFactory)
        {
            _scopeFactory = scopeFactory;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            using var scope = _scopeFactory.CreateScope();

            var boletoRegistradoQueue = scope.ServiceProvider.GetRequiredService<IRegistrarBoletoQueueConsumer>();

            boletoRegistradoQueue.Execute(stoppingToken);

            return Task.CompletedTask;
        }
    }
}