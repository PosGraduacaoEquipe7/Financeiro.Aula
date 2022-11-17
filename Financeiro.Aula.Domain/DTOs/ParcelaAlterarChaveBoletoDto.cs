namespace Financeiro.Aula.Domain.DTOs
{
    public record ParcelaAlterarChaveBoletoDto
    {
        public Guid IdentificadorParcela;

        public string ChaveBoleto;

        public ParcelaAlterarChaveBoletoDto(Guid identificadorParcela, string chaveBoleto)
        {
            IdentificadorParcela = identificadorParcela;
            ChaveBoleto = chaveBoleto;
        }
    }
}