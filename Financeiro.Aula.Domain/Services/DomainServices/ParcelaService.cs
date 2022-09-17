using Financeiro.Aula.Domain.Entities;
using Financeiro.Aula.Domain.Interfaces.DomainServices;

namespace Financeiro.Aula.Domain.Services.DomainServices
{
    public class ParcelaService : IParcelaService
    {
        public List<Parcela> GerarParcelas(double valorTotal, int numeroParcelas, DateTime primeiroVencimento, long contratoId)
        {
            var result = new List<Parcela>();

            for (int i = 1; i <= numeroParcelas; i++)
            {
                var parcela = new Parcela(
                    id: 0,
                    sequencial: i,
                    valor: valorTotal / numeroParcelas,
                    dataVencimento: primeiroVencimento.AddMonths(i - 1),
                    contratoId: contratoId);
            }

            return result;
        }
    }
}