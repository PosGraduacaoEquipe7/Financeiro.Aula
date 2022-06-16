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
        public DbSet<ParametroBoleto> ParametrosBoleto => Set<ParametroBoleto>();
        public DbSet<Parcela> Parcelas => Set<Parcela>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new ClienteMapper());
            modelBuilder.ApplyConfiguration(new ContratoMapper());
            modelBuilder.ApplyConfiguration(new ParametroBoletoMapper());
            modelBuilder.ApplyConfiguration(new ParcelaMapper());

            //modelBuilder.Entity<ParametroBoleto>().HasData(
            //    new ParametroBoleto(
            //        id: 1,
            //        descricao: "Boleto Bradesco",
            //        banco: "237",
            //        agencia: "1234-5",
            //        numeroConta: "123456-0",
            //        carteira: "12",
            //        numeroBoletoAtual: 0,
            //        nomeBeneficiario: "Financeiro Aula Solutions",
            //        cnpjBeneficiario: "09.934.582/0001-58",
            //        enderecoBeneficiario: new Endereco(
            //                cep: "93000-000",
            //                logradouro: "Rua das Empresas",
            //                numero: "112",
            //                complemento: "",
            //                bairro: "Centro",
            //                municipio: "Porto Alegre",
            //                uf: "RS")
            //            ));

            base.OnModelCreating(modelBuilder);
        }
    }
}