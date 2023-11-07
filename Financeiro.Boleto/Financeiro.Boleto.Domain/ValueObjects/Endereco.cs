namespace Financeiro.Boleto.Domain.ValueObjects
{
    public record Endereco
    {
        public string Cep = string.Empty;

        public string Logradouro = string.Empty;

        public string Numero = string.Empty;

        public string? Complemento;

        public string Bairro = string.Empty;

        public string Municipio = string.Empty;

        public string Uf = string.Empty;

        public Endereco(string cep, string logradouro, string numero, string? complemento, string bairro, string municipio, string uf)
        {
            Cep = cep;
            Logradouro = logradouro;
            Numero = numero;
            Complemento = complemento;
            Bairro = bairro;
            Municipio = municipio;
            Uf = uf;
        }
    }
}