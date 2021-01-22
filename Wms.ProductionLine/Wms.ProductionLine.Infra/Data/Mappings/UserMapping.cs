using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Wms.ProductionLine.Domain.Entities;

namespace Wms.ProductionLine.Infra.Data.Mappings
{
    public class UserMapping : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.Property(p => p.WarehouseId).IsRequired(false);
            builder.Property(d => d.Name).IsUnicode(false).HasMaxLength(100).IsRequired();
            builder.Property(d => d.Login).IsUnicode(false).HasMaxLength(100).IsRequired();
        }
    }
}
