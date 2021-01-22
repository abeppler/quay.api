using Hbsis.Wms.Infra.Data.Repository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using Wms.ProductionLine.Domain.Entities;
using Wms.ProductionLine.Domain.Intefaces.Configurations;

namespace Wms.ProductionLine.Infra.Repository.Configurations
{
    public class ProductionLineConfigurationRepository : Repository<ProductionLineConfiguration>, IProductionLineConfigurationRepository
    {
        public ProductionLineConfigurationRepository(DbContext context) : base(context)
        {
        }

        public ProductionLineConfiguration GetByWarehouse(Guid warehouseId)
        {
            return _set.FirstOrDefault(x => x.WarehouseId == warehouseId);
        }

    }
}
