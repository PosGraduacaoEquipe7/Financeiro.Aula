using Financeiro.Aula.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Financeiro.Aula.Infra.Context.Mappers
{
    public class ContratoMapper : IEntityTypeConfiguration<Contrato>
    {
        public void Configure(EntityTypeBuilder<Contrato> builder)
        {
            builder.HasKey(c => c.Id);

            builder.HasMany(c => c.Parcelas)
                   .WithOne(c => c.Contrato)
                   .HasForeignKey(c => c.ContratoId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(c => c.Turma)
                   .WithMany()
                   .HasForeignKey(c => c.TurmaId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}