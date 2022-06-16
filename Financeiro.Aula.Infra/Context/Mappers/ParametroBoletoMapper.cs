using Financeiro.Aula.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Financeiro.Aula.Infra.Context.Mappers
{
    public class ParametroBoletoMapper : IEntityTypeConfiguration<ParametroBoleto>
    {
        public void Configure(EntityTypeBuilder<ParametroBoleto> builder)
        {
            builder.HasKey(c => c.Id);

            builder.OwnsOne(c => c.EnderecoBeneficiario);
        }
    }
}