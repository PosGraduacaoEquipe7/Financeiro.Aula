using Financeiro.Aula.Domain.Interfaces.ApiServices;
using Financeiro.Aula.Domain.Interfaces.Cache;
using Financeiro.Aula.Domain.Interfaces.Services.CEPs;
using Financeiro.Aula.Domain.ValueObjects;

namespace Financeiro.Aula.Domain.Services.DomainServices
{
    public class CepService : ICepService
    {
        private readonly ICepApiService _apiService;
        private readonly ICacheService _cache;
        
        private string CacheKey(string cep) => $"Endereco_Cacheado_{cep}";

        public CepService(ICepApiService apiService, ICacheService cache)
        {
            _apiService = apiService;
            _cache = cache;
        }

        public async Task<Endereco?> BuscarCEP(string cep)
        {
            var enderecoDoCache = await _cache.Get<Endereco>(CacheKey(cep));

            if (enderecoDoCache != null)
                return enderecoDoCache;

            var enderecoDaApi = await _apiService.BuscarCEP(cep);

            if (enderecoDaApi == null)
                return null;

            await _cache.Set(CacheKey(cep), enderecoDaApi);

            return enderecoDaApi;
        }
    }
}