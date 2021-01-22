using Hbsis.Wms.Infra.Domain.Entities;
using System;
using System.Collections.Generic;
using Wms.ProductionLine.Domain.Dto;
using Wms.ProductionLine.Domain.Entities.History;

namespace Wms.ProductionLine.Domain.Entities
{
    public class ProductionLineConfiguration : Entity
    {
        public ProductionLineConfiguration() { }

        public ProductionLineConfiguration(ProductionLineConfigurationDto configurationDto)
        {
            Enabled = configurationDto.Enabled;
            WarehouseId = configurationDto.WarehouseId.Value;
        }

        public bool Enabled { get; private set; }
        public Guid WarehouseId { get; private set; }

        public IList<ProductionLineConfigurationHistory> History { get; private set; }

        public void UpdateProperties(ProductionLineConfigurationDto configurationDto)
        {
            Enabled = configurationDto.Enabled;
            WarehouseId = configurationDto.WarehouseId.Value;
        }
    }
}
