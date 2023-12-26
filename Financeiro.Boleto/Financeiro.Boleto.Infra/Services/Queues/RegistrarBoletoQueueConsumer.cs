using Financeiro.Boleto.Domain.Configuration;
using Financeiro.Boleto.Domain.DTOs;
using Financeiro.Boleto.Domain.Interfaces.Queues;
using Financeiro.Boleto.Domain.Interfaces.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace Financeiro.Boleto.Infra.Services.Queues
{
    public class RegistrarBoletoQueueConsumer : IRegistrarBoletoQueueConsumer
    {
        private readonly ILogger<RegistrarBoletoQueueConsumer> _logger;
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly IBoletoRegistradoQueue _boletoRegistradoQueue;

        private readonly RabbitMqConfiguration _configuration;
        private readonly IConnection _connection;
        private readonly IModel _channel;

        public RegistrarBoletoQueueConsumer(
            ILogger<RegistrarBoletoQueueConsumer> logger,
            IServiceScopeFactory scopeFactory,
            IBoletoRegistradoQueue boletoRegistradoQueue,
            IOptions<RabbitMqConfiguration> option)
        {
            _logger = logger;
            _scopeFactory = scopeFactory;
            _boletoRegistradoQueue = boletoRegistradoQueue;

            _configuration = option.Value;

            _logger.LogInformation("Conectando no RabbitMq em: {host}", _configuration.Host);

            var factory = new ConnectionFactory
            {
                HostName = _configuration.Host
            };

            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();
            _channel.QueueDeclare(
                        queue: _configuration.Queues.RegistrarBoleto,
                        durable: true,
                        exclusive: false,
                        autoDelete: false,
                        arguments: null);
        }

        public async Task Execute(CancellationToken cancellationToken)
        {
            var consumer = new EventingBasicConsumer(_channel);

            consumer.Received += Consumer_Received;

            await Task.Run(() =>
            {
                _channel.BasicConsume(_configuration.Queues.RegistrarBoleto, false, consumer);
            });
        }

        public void Close()
        {
            _channel.Close();
            _connection.Close();
        }

        private async void Consumer_Received(object? sender, BasicDeliverEventArgs eventArgs)
        {
            var contentArray = eventArgs.Body.ToArray();
            var contentString = Encoding.UTF8.GetString(contentArray);

            var boletoDto = JsonConvert.DeserializeObject<BoletoGerarDto>(contentString);

            _logger.LogInformation(
                    "Recebida solicitação do boleto: {boleto} na fila: {fila} - Mensagem: {contentString}",
                    boletoDto?.TokenRetorno,
                    _configuration.Queues.RegistrarBoleto,
                    contentString);

            if (boletoDto is null)
            {
                _logger.LogError("Objeto nulo");
                return;
            }

            using var scope = _scopeFactory.CreateScope();
            var boletoService = scope.ServiceProvider.GetRequiredService<IBoletoService>();
            var boleto = await boletoService.RegistrarBoleto(boletoDto);

            if (boleto is null)
            {
                _channel.BasicNack(eventArgs.DeliveryTag, false, false);
                return;
            }

            _channel.BasicAck(eventArgs.DeliveryTag, false);

            await _boletoRegistradoQueue.EnviarFilaBoletoRegistrado(new BoletoRegistradoDto(boletoDto.TokenRetorno, boleto.Id.ToString()));
        }
    }
}