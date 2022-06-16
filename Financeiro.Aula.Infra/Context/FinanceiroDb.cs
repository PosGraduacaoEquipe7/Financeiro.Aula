using Financeiro.Aula.Domain.Entities;
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

            base.OnModelCreating(modelBuilder);
        }
    }
}