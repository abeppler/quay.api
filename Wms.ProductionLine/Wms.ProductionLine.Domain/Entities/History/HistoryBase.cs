using Hbsis.Wms.Infra.Domain.Entities;
using System;

namespace Wms.ProductionLine.Domain.Entities.History
{
    public class HistoryBase: Entity
    {
        public Guid UserId { get; protected set; }
        public string Description { get; protected set; }
        public DateTime CreatedDate { get; protected set; }
    }
}
