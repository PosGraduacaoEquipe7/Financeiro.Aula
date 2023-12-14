using Financeiro.Auth.Entities;
using Financeiro.Auth.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Financeiro.Auth.Context.Mappers
{
    public class UsuarioMapper : IEntityTypeConfiguration<Usuario>
    {
        public void Configure(EntityTypeBuilder<Usuario> builder)
        {
            builder.HasKey(e => e.Id);
            builder.Property(e => e.Id).ValueGeneratedOnAdd();

            builder.Property(c => c.Senha)
                .HasColumnName(nameof(Usuario.Senha))
                .HasMaxLength(100)
                .HasConversion(
                    x => x.SenhaCifrada,
                    x => Senha.FromHashed(x)
                )
                .IsRequired();

            builder.HasMany(c => c.Acessos)
                   .WithOne(c => c.Usuario)
                   .HasForeignKey(c => c.UsuarioId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}