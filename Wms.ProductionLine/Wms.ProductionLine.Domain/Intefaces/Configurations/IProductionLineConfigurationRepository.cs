using Hbsis.Wms.Infra.Domain.Interfaces.Data;
using System;
using System.Threading.Tasks;
using Wms.ProductionLine.Domain.Entities;

namespace Wms.ProductionLine.Domain.Intefaces.Configurations
{
    public interface IProductionLineConfigurationRepository : IRepository<ProductionLineConfiguration>, IAsyncRepository<ProductionLineConfiguration>
    {
        ProductionLineConfiguration GetByWarehouse (Guid warehouseId);
    }
}
