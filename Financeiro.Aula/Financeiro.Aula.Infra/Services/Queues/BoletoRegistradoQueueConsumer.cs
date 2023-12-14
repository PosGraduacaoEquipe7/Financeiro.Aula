using Financeiro.Aula.Domain.Configurations;
using Financeiro.Aula.Domain.DTOs;
using Financeiro.Aula.Domain.Interfaces.DomainServices;
using Financeiro.Aula.Domain.Interfaces.Queues;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace Financeiro.Aula.Infra.Services.Queues
{
    public class BoletoRegistradoQueueConsumer : IBoletoRegistradoQueueConsumer
    {
        private readonly ILogger<BoletoRegistradoQueueConsumer> _logger;
        private readonly IServiceScopeFactory _scopeFactory;

        private readonly RabbitMqConfiguration _configuration;
        private readonly IConnection _connection;
        private readonly IModel _channel;

        public BoletoRegistradoQueueConsumer(
            ILogger<BoletoRegistradoQueueConsumer> logger,
            IServiceScopeFactory scopeFactory,
            IOptions<RabbitMqConfiguration> option)
        {
            _logger = logger;
            _scopeFactory = scopeFactory;

            _configuration = option.Value;

            _logger.LogInformation("Conectando no RabbitMq em: {host}", _configuration.Host);

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

        public async Task Execute(CancellationToken cancellationToken)
        {
            var consumer = new EventingBasicConsumer(_channel);

            consumer.Received += Consumer_Received;

            await Task.Run(
                    () => _channel.BasicConsume(_configuration.Queues.BoletoRegistrado, false, consumer),
                    cancellationToken
                );
        }

        private async void Consumer_Received(object? sender, BasicDeliverEventArgs eventArgs)
        {
            var contentArray = eventArgs.Body.ToArray();
            var contentString = Encoding.UTF8.GetString(contentArray);

            var boletoDto = JsonConvert.DeserializeObject<BoletoRegistradoDto>(contentString);

            _logger.LogInformation(
                    "Recebido retorno do boleto: {boleto} na fila: {fila} - Mensagem: {contentString}",
                    boletoDto?.Token,
                    _configuration.Queues.BoletoRegistrado,
                    contentString);

            if (boletoDto is null)
            {
                _logger.LogError("Valor nulo");
                return;
            }

            using var scope = _scopeFactory.CreateScope();
            var parcelaService = scope.ServiceProvider.GetRequiredService<IParcelaService>();
            var retorno = await parcelaService.AlterarChaveBoletoParcela(boletoDto.Token, boletoDto.ChaveBoleto);

            if (retorno.Sucesso)
                _channel.BasicAck(eventArgs.DeliveryTag, false);
            else
                _channel.BasicNack(eventArgs.DeliveryTag, false, false);
        }
    }
}