using Financeiro.Aula.Domain.Entities;
using Financeiro.Aula.Domain.Interfaces.Repositories;

namespace Financeiro.Aula.Infra.Repositories
{
    public class EmpresaMockRepository : IEmpresaRepository
    {
        public async Task<Empresa?> ObterEmpresaPadrao()
        {
            var empresa = new Empresa(
                id: 1,
                nomeFantasia: "ABC Instituição de Ensino",
                razaoSocial: "ABC Instituição de Ensino",
                telefone: "(51) 5555-0000",
                endereco: "Rua Teste",
                numeroEndereco: "100",
                complementoEndereco: string.Empty,
                bairro: "Teste",
                municipio: "Porto Alegre",
                uf: "RS",
                cep: "90000-000",
                cnpj: "00.000.000/0000-00"
            );

            return await Task.FromResult<Empresa>(empresa);
        }
    }
}