using Financeiro.Aula.Domain.Entities;
using Financeiro.Aula.Domain.Interfaces.DomainServices;

namespace Financeiro.Aula.Domain.Services.DomainServices
{
    public class ParcelaService : IParcelaService
    {
        public List<Parcela> GerarParcelas(double valorTotal, int numeroParcelas, DateTime primeiroVencimento, long contratoId)
        {
            var result = new List<Parcela>();

            if (numeroParcelas == 0)
                return result;

            double valorParcelas = CalcularValorParcela(valorTotal, numeroParcelas);
            var valorPrimeiraParcela = CalcularValorPrimeiraParcela(valorTotal, numeroParcelas, valorParcelas);

            for (int i = 1; i <= numeroParcelas; i++)
            {
                var parcela = new Parcela(
                    id: 0,
                    sequencial: i,
                    valor: i == 1 ? valorPrimeiraParcela : valorParcelas,
                    dataVencimento: primeiroVencimento.AddMonths(i - 1),
                    contratoId: contratoId);

                result.Add(parcela);
            }

            return result;
        }

        private double CalcularValorParcela(double valorTotal, int numeroParcelas) => Math.Round(valorTotal / numeroParcelas, 2);

        private double CalcularValorPrimeiraParcela(double valorTotal, int numeroParcelas, double valorParcelas) => Math.Round(valorTotal - valorParcelas * (numeroParcelas - 1), 2);
    }
}