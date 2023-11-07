using Financeiro.Aula.Api.Models.Parcelas.Responses;
using Financeiro.Aula.Domain.Entities;

namespace Financeiro.Aula.Api.Models.Parcelas.Mappers
{
    public static class ListarParcelaResponseMapper
    {
        public static IEnumerable<ListarParcelaResponse> Map(IEnumerable<Parcela> parcelas)
        {
            var parcelasMap = parcelas.Select(Map).ToList();

            var primeiraParcela = parcelasMap.OrderBy(p => p.DataVencimento).FirstOrDefault(p => !p.TemBoleto);
            if (primeiraParcela is not null)
                primeiraParcela.GerarBoleto = !primeiraParcela.BoletoPendente;

            return parcelasMap;
        }

        public static ListarParcelaResponse Map(Parcela parcela)
        {
            return new ListarParcelaResponse()
            {
                Id = parcela.Id,
                Sequencial = parcela.Sequencial,
                Valor = parcela.Valor,
                DataVencimento = parcela.DataVencimento,
                TemBoleto = parcela.TemBoleto,
                BoletoPendente = parcela.BoletoPendente
            };
        }
    }
}