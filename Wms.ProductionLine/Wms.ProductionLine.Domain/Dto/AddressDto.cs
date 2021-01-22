using System;

namespace Wms.ProductionLine.Domain.Dto
{
    public class AddressDto
    {
        public Guid Id { get; set; }
        public Guid WarehouseId { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        public bool Enabled { get; set; }
        public int Sequence { get; set; }
    }
}
