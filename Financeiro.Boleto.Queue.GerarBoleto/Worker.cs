using Financeiro.Boleto.Queue.GerarBoleto.Scopes;

namespace Financeiro.Boleto.Queue.GerarBoleto
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly IServiceScope _serviceScope;

        public Worker(ILogger<Worker> logger, IServiceProvider serviceProvider)
        {
            _logger = logger;
            _serviceScope = serviceProvider.CreateScope();
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation($"{nameof(Worker)} is running.");

            await DoWorkAsync(stoppingToken);
        }

        private async Task DoWorkAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation($"{nameof(Worker)} is working.");

            var scopedProcessingService = _serviceScope.ServiceProvider.GetRequiredService<IScopedProcessingService>();

            await scopedProcessingService.DoWorkAsync(stoppingToken);
        }

        public override async Task StopAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation($"{nameof(Worker)} is stopping.");

            await base.StopAsync(stoppingToken);
        }

        public override void Dispose()
        {
            _serviceScope.Dispose();
            base.Dispose();
        }
    }
}