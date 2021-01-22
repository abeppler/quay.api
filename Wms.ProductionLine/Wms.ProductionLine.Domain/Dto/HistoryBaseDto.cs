using System;

namespace Wms.ProductionLine.Domain.Dto
{
    public class HistoryBaseDto
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public TypeHistoryDto HistoryType { get; set; }
        public string Description { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
