using Hbsis.Wms.Infra.Domain.Entities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Wms.ProductionLine.Domain.Dto;

namespace Wms.ProductionLine.Domain.Entities
{
    public class Integralizado: Entity
    {
        public Guid ItemId { get; private set; }
        public Item Item { get; private set; }

        public bool Enabled { get; private set; }
        public Guid? WarehouseId { get; private set; }

        private List<IntegralizadoDetail> _details = new List<IntegralizadoDetail>();
        public IReadOnlyCollection<IntegralizadoDetail> Details => _details.AsReadOnly();

        protected Integralizado()
        {

        }

        public Integralizado(Guid id, Guid itemId, bool enabled, Guid? warehouseId, IList<IntegralizadoDetail> details)
        {
            SetId(id);
            ItemId = itemId;
            Enabled = enabled;
            WarehouseId = warehouseId;
            _details = details.ToList();
        }

        public void UpdateProperties(IntegralizadoDto integralizadoDto)
        {
            ItemId = integralizadoDto.Item;
            Enabled = integralizadoDto.Enabled;
            WarehouseId = integralizadoDto.WarehouseId;
            _details = integralizadoDto.Details.Select(x => new IntegralizadoDetail(x.Id, x.ItemId, x.Quantity)).ToList();
        }
    }
}
