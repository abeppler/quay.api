using System;

namespace Wms.ProductionLine.Domain.Dto
{
    public class ItemDto
    {
        public Guid Id { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        public int ItemType { get; set; }
    }
}
