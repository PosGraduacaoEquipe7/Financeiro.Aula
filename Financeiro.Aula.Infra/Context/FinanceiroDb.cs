using Financeiro.Aula.Domain.Entities;
using Financeiro.Aula.Domain.ValueObjects;
using Financeiro.Aula.Infra.Context.Mappers;
using Microsoft.EntityFrameworkCore;

namespace Financeiro.Aula.Infra.Context
{
    public class FinanceiroDb : DbContext
    {
        public FinanceiroDb(DbContextOptions<FinanceiroDb> options) : base(options) { }

        public DbSet<Cliente> Clientes => Set<Cliente>();
        public DbSet<Contrato> Contratos => Set<Contrato>();
        public DbSet<Parcela> Parcelas => Set<Parcela>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new ClienteMapper());
            modelBuilder.ApplyConfiguration(new ContratoMapper());

            modelBuilder.Entity<Cliente>().HasData(
                new Cliente(
                    id: 1,
                    nome: "José Silva",
                    cpf: "12345678909",
                    endereco: null
                //endereco: new Endereco(
                //        cep: "93000-000",
                //        logradouro: "Rua Um",
                //        numero: "120-A",
                //        complemento: "Fundos",
                //        bairro: "São José",
                //        municipio: "São Leopoldo",
                //        uf: "RS"
                //    )
                ));

            modelBuilder.Entity<Contrato>().HasData(
                new Contrato(
                    id: 1,
                    dataEmissao: DateTime.Now.Date,
                    valorTotal: 1000,
                    clienteId: 1
                ));

            for (int i = 1; i <= 10; i++)
            {
                modelBuilder.Entity<Parcela>().HasData(
                    new Parcela(
                        id: i,
                        sequencial: i,
                        valor: 100,
                        dataVencimento: DateTime.Now.Date.AddMonths(i - 1),
                        contratoId: 1
                    ));
            }

            base.OnModelCreating(modelBuilder);
        }
    }
}