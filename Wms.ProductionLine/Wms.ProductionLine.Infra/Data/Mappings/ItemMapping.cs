using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Wms.ProductionLine.Domain.Entities;

namespace Wms.ProductionLine.Infra.Data.Mappings
{
    public class ItemMapping : IEntityTypeConfiguration<Item>
    {
        public void Configure(EntityTypeBuilder<Item> builder)
        {
            builder.Property(x => x.Description).IsUnicode(false).HasMaxLength(250);
            builder.Property(x => x.Code).IsUnicode(false).HasMaxLength(100);
            builder.Property(x => x.ItemType).IsRequired(false);
        }
    }
}
