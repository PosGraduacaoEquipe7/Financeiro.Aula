using Financeiro.Aula.Domain.Entities;
using Financeiro.Aula.Domain.Interfaces.ApiServices;
using System.Globalization;

namespace Financeiro.Aula.Infra.ApiServices.BoletoCloud
{
    public class IBoletoCloudApiService : IGeradorBoletoApiService
    {
        private readonly HttpClient _client;

        public IBoletoCloudApiService(HttpClient client)
        {
            _client = client;
        }

        public async Task<(bool Sucesso, string Numero, string Token, byte[] Pdf)> GerarBoleto(Parcela parcela, string numeroBoleto)
        {
            var body = MontarBodyDaParcela(parcela, numeroBoleto);

            using (var conteudo = new FormUrlEncodedContent(body))
            {
                conteudo.Headers.Clear();
                conteudo.Headers.Add("Content-Type", "application/x-www-form-urlencoded");

                var response = await _client.PostAsync("boletos", conteudo);

                if (!response.IsSuccessStatusCode)
                {
                    var erro = response.Content.ReadAsStringAsync();
                    return (false, string.Empty, string.Empty, Array.Empty<byte>());
                }

                var token = ObterTokenDoHeader(response);
                var bytes = await response.Content.ReadAsByteArrayAsync();

                return (true, numeroBoleto, token, bytes);
            }
        }

        private static Dictionary<string, string> MontarBodyDaParcela(Parcela parcela, string numeroBoleto)
        {
            var cliente = parcela.Contrato!.Cliente!;

            var body = new Dictionary<string, string>
            {
                ["boleto.conta.banco"] = "237",
                ["boleto.conta.agencia"] = "1234-5",
                ["boleto.conta.numero"] = "123456-0",
                ["boleto.conta.carteira"] = "12",
                ["boleto.beneficiario.nome"] = "Financeiro Aula Solutions",
                ["boleto.beneficiario.cprf"] = "09.934.582/0001-58",
                ["boleto.beneficiario.endereco.cep"] = "59020-000",
                ["boleto.beneficiario.endereco.uf"] = "RS",
                ["boleto.beneficiario.endereco.localidade"] = "Porto Alegre",
                ["boleto.beneficiario.endereco.bairro"] = "Petrópolis",
                ["boleto.beneficiario.endereco.logradouro"] = "Avenida Financeiro",
                ["boleto.beneficiario.endereco.numero"] = "123",
                ["boleto.beneficiario.endereco.complemento"] = "Sala 2A",
                ["boleto.emissao"] = DateTime.Now.ToString("yyyy-MM-dd"),
                ["boleto.vencimento"] = parcela.DataVencimento.ToString("yyyy-MM-dd"),
                ["boleto.documento"] = $"CTR{parcela.ContratoId}",
                ["boleto.numero"] = numeroBoleto,
                ["boleto.titulo"] = "DM",
                ["boleto.valor"] = parcela.Valor.ToString("F").Replace(",", "."),
                ["boleto.pagador.nome"] = cliente.Nome,
                ["boleto.pagador.cprf"] = cliente.Cpf,
                ["boleto.pagador.endereco.cep"] = cliente.Endereco.Cep,
                ["boleto.pagador.endereco.uf"] = cliente.Endereco.Uf,
                ["boleto.pagador.endereco.localidade"] = cliente.Endereco.Municipio,
                ["boleto.pagador.endereco.bairro"] = cliente.Endereco.Bairro,
                ["boleto.pagador.endereco.logradouro"] = cliente.Endereco.Logradouro,
                ["boleto.pagador.endereco.numero"] = cliente.Endereco.Numero,
                ["boleto.pagador.endereco.complemento"] = cliente.Endereco.Complemento,
                ["boleto.instrucao"] = "Atenção! NÃO RECEBER ESTE BOLETO.",
                ["boleto.instrucao"] = "Este é apenas um teste utilizando a API Boleto Cloud",
                ["boleto.instrucao"] = "Mais info em http://boleto.cloud/app/dev/api"
            };

            return body;
        }

        private static string ObterTokenDoHeader(HttpResponseMessage response)
        {
            var token = string.Empty;

            if (response.Headers.Contains("Location"))
            {
                var location = response.Headers.GetValues("Location");

                if (location.Any())
                    token = location.First();
            }

            return token;
        }
    }
}