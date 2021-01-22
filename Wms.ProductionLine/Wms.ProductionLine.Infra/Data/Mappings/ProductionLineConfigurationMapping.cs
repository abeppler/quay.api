using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Wms.ProductionLine.Domain.Entities;

namespace Wms.ProductionLine.Infra.Data.Mappings
{
    public class ProductionLineConfigurationMapping : IEntityTypeConfiguration<ProductionLineConfiguration>
    {
        public void Configure(EntityTypeBuilder<ProductionLineConfiguration> builder)
        {
            builder.Property(d => d.Enabled).IsRequired();
            builder.Property(d => d.WarehouseId).IsRequired();
        }
    }
}
