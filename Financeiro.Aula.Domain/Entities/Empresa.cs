using Financeiro.Aula.Domain.ValueObjects;

namespace Financeiro.Aula.Domain.Entities
{
    public class Empresa
    {
        public long Id { get; private set; }

        public string NomeFantasia { get; private set; }

        public string RazaoSocial { get; private set; }

        public string Telefone { get; private set; }

        public string CNPJ { get; private set; }

        public Endereco Endereco { get; private set; }

        public Empresa(long id, string nomeFantasia, string razaoSocial, string telefone, string endereco, string numeroEndereco, string? complementoEndereco, string bairro, string municipio, string uf, string cep, string cnpj)
        {
            Id = id;
            NomeFantasia = nomeFantasia;
            RazaoSocial = razaoSocial;
            Telefone = telefone;
            CNPJ = cnpj;
            Endereco = new Endereco(cep, endereco, numeroEndereco, complementoEndereco, bairro, municipio, uf);
        }
    }
}