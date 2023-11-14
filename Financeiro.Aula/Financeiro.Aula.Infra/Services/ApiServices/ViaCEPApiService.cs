using System.Net.Http.Json;
using Financeiro.Aula.Domain.Interfaces.ApiServices;
using Financeiro.Aula.Domain.ValueObjects;
using Financeiro.Aula.Infra.Models.Responses;
using Microsoft.Extensions.Logging;

namespace Financeiro.Aula.Infra.Services.ApiServices
{
    public class ViaCEPApiService : ICepApiService
    {
        private readonly HttpClient _client;
        private readonly ILogger<BoletoApiService> _logger;

        public ViaCEPApiService(HttpClient client, ILogger<BoletoApiService> logger)
        {
            _client = client;
            _logger = logger;
        }

        public async Task<Endereco?> BuscarCEP(string cep)
        {
            _logger.LogInformation("Buscando o CEP {cep} na API ViaCEP", cep);

            var response = await _client.GetAsync($"{cep}/json/");

            if (!response.IsSuccessStatusCode)
            {
                var erro = await response.Content.ReadAsStringAsync();
                _logger.LogError("Requisição de obtenção do CEP mal sucedida. {StatusCode} - {Erro}", response.StatusCode, erro);

                return null;
            }

            // TODO: tratar o erro:
            // StatusCode: 200 :/
            /*
            {
                "erro": "true"
            }
            */

            var result = await response.Content.ReadFromJsonAsync<ViaCEPResponse>();

            return MapToEndereco(result);
        }

        private static Endereco? MapToEndereco(ViaCEPResponse? result)
        {
            if (result == null)
                return null;

            return new Endereco(
                cep: result.CEP,
                logradouro: result.Logradouro,
                numero: string.Empty,
                complemento: result.Complemento,
                bairro: result.Bairro,
                municipio: result.Localidade,
                uf: result.UF);
        }
    }
}