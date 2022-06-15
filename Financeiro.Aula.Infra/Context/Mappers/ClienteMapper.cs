using Financeiro.Aula.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Financeiro.Aula.Infra.Context.Mappers
{
    public class ClienteMapper : IEntityTypeConfiguration<Cliente>
    {
        public void Configure(EntityTypeBuilder<Cliente> builder)
        {
            builder.HasKey(c => c.Id);

            builder.HasMany(c => c.Contratos)
                   .WithOne(c => c.Cliente)
                   .HasForeignKey(c => c.ClienteId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.OwnsOne(c => c.Endereco);
        }
    }
}