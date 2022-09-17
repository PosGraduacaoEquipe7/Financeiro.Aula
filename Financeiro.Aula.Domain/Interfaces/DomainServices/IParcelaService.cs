using Financeiro.Aula.Domain.Entities;

namespace Financeiro.Aula.Domain.Interfaces.DomainServices
{
    public interface IParcelaService
    {
        List<Parcela> GerarParcelas(double valorTotal, int numeroParcelas, DateTime primeiroVencimento, long contratoId);
    }
}