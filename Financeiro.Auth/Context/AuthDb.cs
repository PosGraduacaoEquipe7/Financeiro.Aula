using Financeiro.Auth.Context.Mappers;
using Financeiro.Auth.Entities;
using Microsoft.EntityFrameworkCore;

namespace Financeiro.Auth.Context
{
    public class AuthDb : DbContext
    {
        public AuthDb(DbContextOptions<AuthDb> options) : base(options) { }
        
        public DbSet<Acesso> Acessos => Set<Acesso>();
        public DbSet<Usuario> Usuarios => Set<Usuario>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new UsuarioMapper());

            base.OnModelCreating(modelBuilder);
        }
    }
}