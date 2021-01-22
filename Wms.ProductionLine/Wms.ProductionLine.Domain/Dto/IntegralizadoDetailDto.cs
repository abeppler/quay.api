using System;
using System.Collections.Generic;
using System.Text;

namespace Wms.ProductionLine.Domain.Dto
{
    public class IntegralizadoDetailDto
    {
        public Guid Id { get; set; }
        public Guid ItemId { get; set; }
        public double Quantity { get; set; }

        public IntegralizadoDetailDto(Guid id, Guid itemId, double quantity)
        {
            Id = id;
            ItemId = itemId;
            Quantity = quantity;
        }
    }
}
