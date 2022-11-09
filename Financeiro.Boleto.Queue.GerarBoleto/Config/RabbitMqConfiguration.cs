namespace Financeiro.Boleto.Queue.GerarBoleto.Config
{
    public class RabbitMqConfiguration
    {
        public string Host { get; set; } = string.Empty;
        public string Queue { get; set; } = string.Empty;
        //public string Username { get; set; }
        //public string Password { get; set; }
    }
}