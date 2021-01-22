using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using Wms.ProductionLine.Domain.Entities.History;

namespace Wms.ProductionLine.Infra.Data.Mappings
{
    public class ProductionLineConfigurationHistoryMapping : IEntityTypeConfiguration<ProductionLineConfigurationHistory>
    {
        public void Configure(EntityTypeBuilder<ProductionLineConfigurationHistory> builder)
        {
            builder.HasOne(x => x.ProductionLineConfiguration)
                .WithMany(x => x.History)
                .HasForeignKey(x => x.ProductionLineConfigurationId)
                .IsRequired();
        }
    }
}
