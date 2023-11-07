using Financeiro.Boleto.Domain.Entities;
using Financeiro.Boleto.Domain.ValueObjects;
using Financeiro.Boleto.Infra.Context.Mappers;
using Microsoft.EntityFrameworkCore;

namespace Financeiro.Boleto.Infra.Context
{
    public class BoletoDb : DbContext
    {
        //private readonly string _connectionString;

        public BoletoDb(DbContextOptions<BoletoDb> options) : base(options) { }
        //public BoletoDb(string connectionString)
        //{
        //    _connectionString = connectionString;
        //}

        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    optionsBuilder.UseSqlServer(_connectionString);
        //}

        public DbSet<Domain.Entities.Boleto> Boletos => Set<Domain.Entities.Boleto>();
        public DbSet<ParametroBoleto> ParametrosBoleto => Set<ParametroBoleto>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new BoletoMapper());
            modelBuilder.ApplyConfiguration(new ParametroBoletoMapper());

            //modelBuilder.Entity<ParametroBoleto>()
            //    .OwnsOne(p => p.EnderecoBeneficiario)
            //    .HasData(
            //        new ParametroBoleto(
            //            id: 1,
            //            descricao: "Boleto Bradesco",
            //            banco: "237",
            //            agencia: "1234-5",
            //            numeroConta: "123456-0",
            //            carteira: "12",
            //            numeroBoletoAtual: 0,
            //            nomeBeneficiario: "Financeiro Aula Solutions",
            //            cnpjBeneficiario: "09.934.582/0001-58",
            //            enderecoBeneficiario: new Endereco(
            //                cep: "93000-000",
            //                logradouro: "Rua das Empresas",
            //                numero: "112",
            //                complemento: "",
            //                bairro: "Centro",
            //                municipio: "Porto Alegre",
            //                uf: "RS")
            //            )
            //);

            base.OnModelCreating(modelBuilder);
        }
    }
}