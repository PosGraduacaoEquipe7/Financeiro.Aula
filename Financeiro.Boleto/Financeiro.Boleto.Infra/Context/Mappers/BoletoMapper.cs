using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Financeiro.Boleto.Infra.Context.Mappers
{
    public class BoletoMapper : IEntityTypeConfiguration<Domain.Entities.Boleto>
    {
        public void Configure(EntityTypeBuilder<Domain.Entities.Boleto> builder)
        {
            builder.HasKey(c => c.Id);

            builder.OwnsOne(c => c.Endereco, (e) =>
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