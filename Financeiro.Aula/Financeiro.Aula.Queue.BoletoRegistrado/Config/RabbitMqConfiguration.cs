namespace Financeiro.Aula.Queue.BoletoRegistrado.Config
{
    public class RabbitMqConfiguration
    {
        public string Host { get; set; } = string.Empty;
        public RabbitMqQueuesConfiguration Queues { get; set; } = null!;
        //public string Username { get; set; }
        //public string Password { get; set; }
    }

    public class RabbitMqQueuesConfiguration
    {
        public string RegistrarBoleto { get; set; } = string.Empty;
        public string BoletoRegistrado { get; set; } = string.Empty;
    }
}