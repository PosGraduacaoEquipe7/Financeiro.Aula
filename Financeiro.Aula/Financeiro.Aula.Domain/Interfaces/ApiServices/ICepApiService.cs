using Financeiro.Aula.Domain.ValueObjects;

namespace Financeiro.Aula.Domain.Interfaces.ApiServices
{
    public interface ICepApiService
    {
        Task<Endereco?> BuscarCEP(string cep);
    }
}