using System;
using System.Collections.Generic;
using System.Text;

namespace Wms.ProductionLine.Domain.Dto
{
    public class ProductionLineConfigurationHistoryDto
    {
        public Guid UserId { get; set; }
        public TypeHistoryDto HistoryType { get; set; }
        public string Description { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
