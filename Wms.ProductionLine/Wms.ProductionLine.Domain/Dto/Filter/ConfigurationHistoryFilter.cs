using System;

namespace Wms.ProductionLine.Domain.Dto.Filter
{
    public class ConfigurationHistoryFilter: FilterBase
    {
        public Guid ConfigurationId { get; set; }
    }
}
