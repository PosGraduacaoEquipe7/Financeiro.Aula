using Financeiro.Aula.Domain.DTOs;
using Financeiro.Aula.Domain.Entities;
using Financeiro.Aula.Domain.Interfaces.ApiServices;
using Financeiro.Aula.Domain.Interfaces.Repositories;

namespace Financeiro.Aula.Infra.ApiServices.BoletoCloud
{
    public class BoletoCloudApiService : IGeradorBoletoApiService
    {
        private readonly HttpClient _client;
        private readonly ParametroBoleto? _parametroBoleto;

        public BoletoCloudApiService(HttpClient client, IParametroBoletoRepository parametroBoletoRepository)
        {
            _client = client;
            _parametroBoleto = parametroBoletoRepository.ObterParametrosBoleto().Result;
        }

        public async Task<(bool Sucesso, byte[] Pdf)> ObterPdfBoleto(Parcela parcela)
        {
            if (parcela.ChaveBoleto is null)
                return (false, Array.Empty<byte>());

            var response = await _client.GetAsync($"boletos/{parcela.ChaveBoleto}");

            if (!response.IsSuccessStatusCode)
            {
                //var erro = response.Content.ReadAsStringAsync();
                return new(false, Array.Empty<byte>());
            }

            var bytes = await response.Content.ReadAsByteArrayAsync();

            return new(true, bytes);
        }

        public async Task<CriacaoBoletoDto> GerarBoleto(Parcela parcela)
        {
            if (_parametroBoleto is null)
                return new(false, string.Empty, string.Empty, Array.Empty<byte>());

            var numeroBoleto = _parametroBoleto.ObterProximoNumeroFormatado();

            var body = MontarBodyDaParcela(parcela, numeroBoleto);

            using (var conteudo = new FormUrlEncodedContent(body))
            {
                conteudo.Headers.Clear();
                conteudo.Headers.Add("Content-Type", "application/x-www-form-urlencoded");

                var response = await _client.PostAsync("boletos", conteudo);

                if (!response.IsSuccessStatusCode)
                {
                    //var erro = response.Content.ReadAsStringAsync();
                    return new(false, string.Empty, string.Empty, Array.Empty<byte>());
                }

                var token = ObterTokenDoHeader(response);
                var bytes = await response.Content.ReadAsByteArrayAsync();

                return new(true, numeroBoleto, token, bytes);
            }
        }

        private Dictionary<string, string> MontarBodyDaParcela(Parcela parcela, string numeroBoleto)
        {
            var cliente = parcela.Contrato!.Cliente!;

            var body = new Dictionary<string, string>
            {
                ["boleto.conta.banco"] = _parametroBoleto!.Banco,
                ["boleto.conta.agencia"] = _parametroBoleto.Agencia,
                ["boleto.conta.numero"] = _parametroBoleto.NumeroConta,
                ["boleto.conta.carteira"] = _parametroBoleto.Carteira,
                ["boleto.beneficiario.nome"] = _parametroBoleto.NomeBeneficiario,
                ["boleto.beneficiario.cprf"] = _parametroBoleto.CnpjBeneficiario,
                ["boleto.beneficiario.endereco.cep"] = _parametroBoleto.EnderecoBeneficiario.Cep,
                ["boleto.beneficiario.endereco.uf"] = _parametroBoleto.EnderecoBeneficiario.Uf,
                ["boleto.beneficiario.endereco.localidade"] = _parametroBoleto.EnderecoBeneficiario.Municipio,
                ["boleto.beneficiario.endereco.bairro"] = _parametroBoleto.EnderecoBeneficiario.Bairro,
                ["boleto.beneficiario.endereco.logradouro"] = _parametroBoleto.EnderecoBeneficiario.Logradouro,
                ["boleto.beneficiario.endereco.numero"] = _parametroBoleto.EnderecoBeneficiario.Numero,
                ["boleto.beneficiario.endereco.complemento"] = _parametroBoleto.EnderecoBeneficiario.Complemento,
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
            if (response.Headers.Contains("Location"))
            {
                var location = response.Headers.GetValues("Location");

                if (location.Any())
                {
                    var token = location.First();

                    return token.Split('/').Last();
                }
            }

            return String.Empty;
        }
    }
}