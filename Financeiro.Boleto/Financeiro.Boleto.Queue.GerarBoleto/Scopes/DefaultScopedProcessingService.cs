using Financeiro.Boleto.Domain.DTOs;
using Financeiro.Boleto.Domain.Interfaces.Services;
using Financeiro.Boleto.Domain.Services.Queues;
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
        private readonly ILogger<DefaultScopedProcessingService> _logger;
        private readonly IBoletoService _boletoService;
        //private readonly IBoletoRegistradoQueue _boletoRegistradoQueue;

        private readonly RabbitMqConfiguration _configuration;
        private readonly IConnection _connection;
        private readonly IModel _channel;

        public DefaultScopedProcessingService(
            ILogger<DefaultScopedProcessingService> logger,
            IBoletoService boletoService,
            //IBoletoRegistradoQueue boletoRegistradoQueue,
            IOptions<RabbitMqConfiguration> option)
        {
            _logger = logger;
            _boletoService = boletoService;
            //_boletoRegistradoQueue = boletoRegistradoQueue;

            _configuration = option.Value;

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

        public async Task DoWorkAsync(CancellationToken stoppingToken)
        {
            var consumer = new EventingBasicConsumer(_channel);

            consumer.Received += Consumer_Received;

            await Task.Run(() =>
            {
                _channel.BasicConsume(_configuration.Queues.RegistrarBoleto, false, consumer);
            });
        }

        private async void Consumer_Received(object? sender, BasicDeliverEventArgs eventArgs)
        {
            var contentArray = eventArgs.Body.ToArray();
            var contentString = Encoding.UTF8.GetString(contentArray);

            _logger.LogInformation("Recebido na fila: {fila} - Mensagem: {contentString}", _configuration.Queues.RegistrarBoleto, contentString);

            var boletoDto = JsonConvert.DeserializeObject<BoletoGerarDto>(contentString);

            if (boletoDto is null)
            {
                _logger.LogError("Valor nulo");
                return;
            }

            var boleto = await _boletoService.RegistrarBoleto(boletoDto);

            if (boleto is null)
            {
                _channel.BasicNack(eventArgs.DeliveryTag, false, false);
                return;
            }

            _channel.BasicAck(eventArgs.DeliveryTag, false);

            //await _boletoRegistradoQueue.EnviarFilaBoletoRegistrado(new BoletoRegistradoDto(boletoDto.TokenRetorno, boleto.ChaveBoleto));
        }
    }
}