namespace Financeiro.Aula.Domain.ValueObjects
{
    public class Endereco
    {
        public string Cep { get; private set; }

        public string Logradouro { get; private set; }

        public string Numero { get; private set; }

        public string Complemento { get; private set; }

        public string Bairro { get; private set; }

        public string Municipio { get; private set; }

        public string Uf { get; private set; }

        private Endereco()
        {
        }

        public Endereco(string cep, string logradouro, string numero, string complemento, string bairro, string municipio, string uf)
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