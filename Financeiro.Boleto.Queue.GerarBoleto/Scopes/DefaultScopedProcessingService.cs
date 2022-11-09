using Financeiro.Boleto.Domain.DTOs;
using Financeiro.Boleto.Domain.Interfaces.Services;
using Financeiro.Boleto.Queue.GerarBoleto.Config;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace Financeiro.Boleto.Queue.GerarBoleto.Scopes
{
    public class DefaultScopedProcessingService : IScopedProcessingService
    {
        private int _executionCount;
        private readonly ILogger<DefaultScopedProcessingService> _logger;
        private readonly IBoletoService _boletoService;

        private readonly RabbitMqConfiguration _configuration;
        private readonly IConnection _connection;
        private readonly IModel _channel;
        private readonly IServiceProvider _serviceProvider;

        public DefaultScopedProcessingService(
            ILogger<DefaultScopedProcessingService> logger,
            IBoletoService boletoService,
            IOptions<RabbitMqConfiguration> option,
            IServiceProvider serviceProvider)
        {
            _logger = logger;
            _boletoService = boletoService;

            _configuration = option.Value;
            _serviceProvider = serviceProvider;

            var factory = new ConnectionFactory
            {
                HostName = _configuration.Host
            };

            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();
            _channel.QueueDeclare(
                        queue: _configuration.Queue,
                        durable: true,
                        exclusive: false,
                        autoDelete: false,
                        arguments: null);
        }

        public async Task DoWorkAsync(CancellationToken stoppingToken)
        {
            var consumer = new EventingBasicConsumer(_channel);

            consumer.Received += async (sender, eventArgs) =>
            {
                var contentArray = eventArgs.Body.ToArray();
                var contentString = Encoding.UTF8.GetString(contentArray);

                _logger.LogInformation(contentString);

                var boletoDto = JsonConvert.DeserializeObject<BoletoGerarDto>(contentString);

                if (boletoDto is null)
                {
                    _logger.LogError("Valor nulo");
                    return;
                }

                await _boletoService.RegistrarBoleto(boletoDto);

                _channel.BasicAck(eventArgs.DeliveryTag, false);
            };

            await Task.Run(() =>
            {
                _channel.BasicConsume(_configuration.Queue, false, consumer);
            });

            // TODO: gambiarra BRABA pro contexto de quem chamou não dar dispose
            while (!stoppingToken.IsCancellationRequested)
            {
                await Task.Delay(3, stoppingToken);
            }
        }
    }
}