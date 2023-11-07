using Financeiro.Boleto.Domain.Entities;

namespace Financeiro.Boleto.Domain.Interfaces.Repositories
{
    public interface IParametroBoletoRepository
    {
        Task<ParametroBoleto?> ObterParametrosBoleto();
    }
}