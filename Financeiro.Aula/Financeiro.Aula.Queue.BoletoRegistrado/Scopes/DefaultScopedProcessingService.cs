using Financeiro.Aula.Domain.DTOs;
using Financeiro.Aula.Domain.Interfaces.DomainServices;
using Financeiro.Aula.Queue.BoletoRegistrado.Config;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace Financeiro.Aula.Queue.BoletoRegistrado.Scopes
{
    public class DefaultScopedProcessingService : IScopedProcessingService
    {
        private readonly ILogger<DefaultScopedProcessingService> _logger;
        private readonly IParcelaService _parcelaService;

        private readonly RabbitMqConfiguration _configuration;
        private readonly IConnection _connection;
        private readonly IModel _channel;

        public DefaultScopedProcessingService(
            ILogger<DefaultScopedProcessingService> logger,
            IParcelaService parcelaService,
            IOptions<RabbitMqConfiguration> option)
        {
            _logger = logger;
            _parcelaService = parcelaService;

            _configuration = option.Value;

            var factory = new ConnectionFactory
            {
                HostName = _configuration.Host
            };

            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();
            _channel.QueueDeclare(
                        queue: _configuration.Queues.BoletoRegistrado,
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

                _logger.LogInformation("Recebido na fila: {fila} - Mensagem: {contentString}", _configuration.Queues.BoletoRegistrado, contentString);

                var boletoDto = JsonConvert.DeserializeObject<BoletoRegistradoDto>(contentString);

                if (boletoDto is null)
                {
                    _logger.LogError("Valor nulo");
                    return;
                }

                var retorno = await _parcelaService.AlterarChaveBoletoParcela(boletoDto.Token, boletoDto.ChaveBoleto);

                if (retorno.Sucesso)
                    _channel.BasicAck(eventArgs.DeliveryTag, false);
                else
                    _channel.BasicNack(eventArgs.DeliveryTag, false, false);
            };

            await Task.Run(() =>
            {
                _channel.BasicConsume(_configuration.Queues.BoletoRegistrado, false, consumer);
            });
        }
    }
}