using System;
using System.Collections.Generic;
using System.Text;
using Wms.ProductionLine.Domain.Entities;

namespace Wms.ProductionLine.Domain.Dto
{
    public class ProductionLineConfigurationDto
    {
        public ProductionLineConfigurationDto()
        {

        }
        public ProductionLineConfigurationDto(ProductionLineConfiguration configuration)
        {
            Id = configuration.Id;
            Enabled = configuration.Enabled;
            WarehouseId = configuration.WarehouseId;
        }

        public Guid? Id { get; set; }
        public Guid? WarehouseId { get; set; }
        public bool Enabled { get; set; }
        public string UserLogin { get; set; }
        public Guid UserId { get; set; }
    }
}
