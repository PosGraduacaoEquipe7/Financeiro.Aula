using Financeiro.Aula.Domain.Interfaces.ApiServices;
using Financeiro.Aula.Domain.Interfaces.Services.CEPs;
using Financeiro.Aula.Domain.ValueObjects;

namespace Financeiro.Aula.Domain.Services.DomainServices
{
    public class CepService : ICepService
    {
        private readonly ICepApiService _apiService;

        public CepService(ICepApiService apiService)
        {
            _apiService = apiService;
        }

        public Task<Endereco?> BuscarCEP(string cep)
        {
            return _apiService.BuscarCEP(cep);
        }
    }
}