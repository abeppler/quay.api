using Hbsis.Wms.Infra.Domain.Entities;
using System;

namespace Wms.ProductionLine.Domain.Entities
{
    public class IntegralizadoDetail: Entity
    {
        public Guid ItemId { get; private set; }
        public Item Item { get; private set; }

        public double Quantity { get; set; }

        public Guid IntegralizadoId { get; private set; }
        public Integralizado Integralizado { get; private set; }

        protected IntegralizadoDetail()
        {
        }

        public IntegralizadoDetail(Guid id, Guid itemId, double quantity)
        {
            SetId(id);
            ItemId = itemId;
            Quantity = quantity;
        }
    }
}
