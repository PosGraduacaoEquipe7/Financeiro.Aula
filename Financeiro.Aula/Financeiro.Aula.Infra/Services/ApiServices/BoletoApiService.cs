using Financeiro.Aula.Domain.Interfaces.ApiServices;
using Microsoft.Extensions.Logging;

namespace Financeiro.Aula.Infra.Services.ApiServices
{
    public class BoletoApiService : IBoletoApiService
    {
        private readonly HttpClient _client;
        private readonly ILogger<BoletoApiService> _logger;

        public BoletoApiService(HttpClient client, ILogger<BoletoApiService> logger)
        {
            _client = client;
            _logger = logger;
        }

        public async Task<string?> ObterPdfBoleto(string chaveBoleto)
        {
            var response = await _client.GetAsync($"api/Boletos/{chaveBoleto}/pdf");

            if (!response.IsSuccessStatusCode)
            {
                var erro = await response.Content.ReadAsStringAsync();
                _logger.LogError("Requisição de obtenção de boleto mal sucedida. {StatusCode} - {Erro}", response.StatusCode, erro);

                return null;
            }

            return await response.Content.ReadAsStringAsync();
        }
    }
}