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
        public DbSet<Curso> Cursos => Set<Curso>();
        public DbSet<ParametroBoleto> ParametrosBoleto => Set<ParametroBoleto>();
        public DbSet<Parcela> Parcelas => Set<Parcela>();
        public DbSet<Turma> Turmas => Set<Turma>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new ClienteMapper());
            modelBuilder.ApplyConfiguration(new ContratoMapper());
            modelBuilder.ApplyConfiguration(new CursoMapper());
            modelBuilder.ApplyConfiguration(new ParametroBoletoMapper());
            modelBuilder.ApplyConfiguration(new ParcelaMapper());
            modelBuilder.ApplyConfiguration(new TurmaMapper());

            base.OnModelCreating(modelBuilder);
        }
    }
}