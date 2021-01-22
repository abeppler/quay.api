using System;
using System.Collections.Generic;
using System.Text;
using Wms.ProductionLine.Domain.Entities;

namespace Wms.ProductionLine.Domain.Dto
{
    public class IntegralizadoDto
    {
        public Guid Id { get; set; }
        public Guid Item { get; set; }
        public bool Enabled { get; set; }
        public Guid? WarehouseId { get; set; }
        public IList<IntegralizadoDetailDto> Details { get; set; }
    }
}
