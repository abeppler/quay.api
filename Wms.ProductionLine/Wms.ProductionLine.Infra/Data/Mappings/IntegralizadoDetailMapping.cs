using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Wms.ProductionLine.Domain.Entities;

namespace Wms.ProductionLine.Infra.Data.Mappings
{
    public class IntegralizadoDetailMapping : IEntityTypeConfiguration<IntegralizadoDetail>
    {
        public void Configure(EntityTypeBuilder<IntegralizadoDetail> builder)
        {
            builder.HasOne(x => x.Integralizado).WithMany(x => x.Details);
            builder.HasOne(x => x.Item).WithMany().OnDelete(DeleteBehavior.Restrict);
        }
    }
}
