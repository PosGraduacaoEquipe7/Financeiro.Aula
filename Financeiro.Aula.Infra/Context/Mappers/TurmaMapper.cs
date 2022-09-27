using Financeiro.Aula.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Financeiro.Aula.Infra.Context.Mappers
{
    public class TurmaMapper : IEntityTypeConfiguration<Turma>
    {
        public void Configure(EntityTypeBuilder<Turma> builder)
        {
            builder.HasKey(c => c.Id);

            builder.HasOne(c => c.Curso)
                   .WithMany()
                   .HasForeignKey(c => c.CursoId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
            