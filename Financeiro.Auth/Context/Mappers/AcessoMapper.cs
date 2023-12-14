using Financeiro.Auth.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Financeiro.Auth.Context.Mappers
{
    public class AcessoMapper: IEntityTypeConfiguration<Acesso>
    {
        public void Configure(EntityTypeBuilder<Acesso> builder)
        {
            builder.HasKey(e => e.Id);
            builder.Property(e => e.Id).ValueGeneratedOnAdd();
        }
    }
}