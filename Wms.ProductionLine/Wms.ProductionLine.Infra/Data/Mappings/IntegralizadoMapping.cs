using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Wms.ProductionLine.Domain.Entities;

namespace Wms.ProductionLine.Infra.Data.Mappings
{
    public class IntegralizadoMapping : IEntityTypeConfiguration<Integralizado>
    {
        public void Configure(EntityTypeBuilder<Integralizado> builder)
        {
            builder.HasOne(x => x.Item).WithMany().OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(x => x.Details).WithOne(x => x.Integralizado);

            var navigation = builder.Metadata.FindNavigation(nameof(Integralizado.Details));
            navigation.SetPropertyAccessMode(PropertyAccessMode.Field);
        }
    }
}
