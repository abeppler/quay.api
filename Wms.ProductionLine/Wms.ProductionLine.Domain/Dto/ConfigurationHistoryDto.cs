using System;

namespace Wms.ProductionLine.Domain.Dto
{
    public class ConfigurationHistoryDto: HistoryBaseDto
    {
        public Guid ConfigurationId { get; set; }
    }
}
