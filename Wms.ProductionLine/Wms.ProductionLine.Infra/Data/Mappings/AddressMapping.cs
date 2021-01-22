using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Wms.ProductionLine.Domain.Entities;

namespace Wms.ProductionLine.Infra.Data.Mappings
{
    public class AddressMapping : IEntityTypeConfiguration<Address>
    {
        public void Configure(EntityTypeBuilder<Address> builder)
        {
            builder.Property(p => p.WarehouseId).IsRequired();
            builder.Property(p => p.Code).IsUnicode(false).HasMaxLength(30);
            builder.Property(p => p.Description).IsUnicode(false).HasMaxLength(250);
            builder.Property(p => p.Enabled);
            builder.Property(p => p.Sequence);
        }
    }
}
