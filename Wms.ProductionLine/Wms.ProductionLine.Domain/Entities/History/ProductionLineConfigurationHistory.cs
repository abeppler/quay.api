using System;

namespace Wms.ProductionLine.Domain.Entities.History
{
    public class ProductionLineConfigurationHistory : HistoryBase
    {
        public ProductionLineConfigurationHistory()
        {
        }

        public ProductionLineConfigurationHistory(Guid userId, string description, Guid configurationId, ConfigurationHistoryType historyType, DateTime createdDate)
        {
            UserId = userId;
            Description = description;
            ProductionLineConfigurationId = configurationId;
            HistoryType = historyType;
            CreatedDate = createdDate;
        }

        public Guid ProductionLineConfigurationId { get; protected set; }
        public ProductionLineConfiguration ProductionLineConfiguration { get; protected set; }
        public ConfigurationHistoryType HistoryType { get; protected set; }
    }
}
