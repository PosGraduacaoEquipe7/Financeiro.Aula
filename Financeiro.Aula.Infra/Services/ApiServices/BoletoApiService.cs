using Financeiro.Aula.Domain.Interfaces.ApiServices;

namespace Financeiro.Aula.Infra.Services.ApiServices
{
    public class BoletoApiService : IBoletoApiService
    {
        private readonly HttpClient _client;

        public BoletoApiService(HttpClient client)
        {
            _client = client;
        }

        public async Task<string?> ObterPdfBoleto(long parcelaId)
        {
            // TODO: chamar antes um service para obter a parcela e a chave do boleto

            var response = await _client.GetAsync($"boletos/{chaveBoleto}");

            if (!response.IsSuccessStatusCode)
            {
                var erro = await response.Content.ReadAsStringAsync();
                _logger.LogError("Requisição de obtenção de boleto mal sucedida. {StatusCode} - {Erro}", response.StatusCode, erro);

                return null;
            }

            return await response.Content.ReadAsByteArrayAsync();
        }
    }
}