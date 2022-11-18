using Financeiro.Aula.Api.Models.Parcelas.Responses;
using Financeiro.Aula.Domain.Entities;

namespace Financeiro.Aula.Api.Models.Parcelas.Mappers
{
    public static class ListarParcelaResponseMapper
    {
        public static IEnumerable<ListarParcelaResponse> Map(IEnumerable<Parcela> parcelas)
            => parcelas.Select(Map);

        public static ListarParcelaResponse Map(Parcela parcela)
        {
            return new ListarParcelaResponse()
            {
                Id = parcela.Id,
                Sequencial = parcela.Sequencial,
                Valor = parcela.Valor,
                DataVencimento = parcela.DataVencimento,
                TemBoleto = parcela.TemBoleto
            };
        }
    }
}