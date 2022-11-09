using Financeiro.Boleto.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Financeiro.Boleto.Infra.Context.Mappers
{
    public class ParametroBoletoMapper : IEntityTypeConfiguration<ParametroBoleto>
    {
        public void Configure(EntityTypeBuilder<ParametroBoleto> builder)
        {
            builder.HasKey(c => c.Id);
            builder.Property(p => p.Id)
                .ValueGeneratedNever();

            builder.OwnsOne(c => c.EnderecoBeneficiario, (e) =>
            {
                e.Property(e => e.Cep).IsRequired();
                e.Property(e => e.Logradouro).IsRequired();
                e.Property(e => e.Numero).IsRequired();
                e.Property(e => e.Complemento);
                e.Property(e => e.Bairro).IsRequired();
                e.Property(e => e.Municipio).IsRequired();
                e.Property(e => e.Uf).IsRequired();
            });
        }
    }
}