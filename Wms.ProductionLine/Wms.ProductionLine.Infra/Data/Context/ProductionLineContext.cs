using Microsoft.EntityFrameworkCore;
using Wms.ProductionLine.Infra.Data.Mappings;

namespace Wms.ProductionLine.Infra.Data.Context
{
    public class ProductionLineContext : DbContext
    {
        public ProductionLineContext() { }

        public ProductionLineContext(DbContextOptions<ProductionLineContext> options) : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("DefaultConnection");
            }
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.ApplyConfiguration(new UserMapping());
            builder.ApplyConfiguration(new AddressMapping());
            builder.ApplyConfiguration(new ProductionLineConfigurationMapping());
            builder.ApplyConfiguration(new ItemMapping());
            builder.ApplyConfiguration(new IntegralizadoMapping());
            builder.ApplyConfiguration(new IntegralizadoDetailMapping());
        }
    }
}
