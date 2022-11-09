using Financeiro.Aula.Domain.DTOs;
using Financeiro.Aula.Domain.Interfaces.Queues;
using Newtonsoft.Json;
using RabbitMQ.Client;
using System.Text;

namespace Financeiro.Aula.Domain.Services.Queues
{
    public class BoletoQueue : IBoletoQueue
    {
        public Task EnviarParcelaFilaGerarBoleto(ParcelaGerarBoletoDto parcelaDto)
        {
            var factory = new ConnectionFactory() { HostName = "localhost" };
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.QueueDeclare(queue: "hello",
                                     durable: true,
                                     exclusive: false,
                                     autoDelete: false,
                                     arguments: null);

                string message = JsonConvert.SerializeObject(parcelaDto);

                var body = Encoding.UTF8.GetBytes(message);

                channel.BasicPublish(exchange: "",
                                     routingKey: "hello",
                                     basicProperties: null,
                                     body: body);

                Console.WriteLine(" [x] Sent {0}", message);
            }

            return Task.CompletedTask;
        }
    }
}