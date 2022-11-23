using Financeiro.Aula.Domain.Configurations;
using Financeiro.Aula.Domain.DTOs;
using Financeiro.Aula.Domain.Interfaces.Queues;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using RabbitMQ.Client;
using System.Text;

namespace Financeiro.Aula.Infra.Services.Queues
{
    public class RegistrarBoletoQueue : IRegistrarBoletoQueue
    {
        private readonly ILogger<RegistrarBoletoQueue> _logger;
        private readonly RabbitMqConfiguration _configuration;
        private readonly IConnection _connection;
        private readonly IModel _channel;

        public RegistrarBoletoQueue(
            ILogger<RegistrarBoletoQueue> logger,
            IOptions<RabbitMqConfiguration> option)
        {
            _logger = logger;

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

        public Task EnviarParcelaFilaGerarBoleto(ParcelaGerarBoletoDto parcelaDto)
        {
            string message = JsonConvert.SerializeObject(parcelaDto);

            var body = Encoding.UTF8.GetBytes(message);

            _channel.BasicPublish(exchange: "",
                                 routingKey: _configuration.Queues.RegistrarBoleto,
                                 basicProperties: null,
                                 body: body);

            _logger.LogInformation("Enviado mensagem para fila: {fila} - Mensagem: {message}", _configuration.Queues.RegistrarBoleto, message);

            return Task.CompletedTask;
        }
    }
}