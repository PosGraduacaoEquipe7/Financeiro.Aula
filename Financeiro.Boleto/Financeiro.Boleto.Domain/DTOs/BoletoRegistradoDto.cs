namespace Financeiro.Boleto.Domain.DTOs
{
    public record BoletoRegistradoDto
    {
        public Guid Token;

        public string ChaveBoleto;

        public BoletoRegistradoDto(Guid token, string chaveBoleto)
        {
            Token = token;
            ChaveBoleto = chaveBoleto;
        }
    }
}