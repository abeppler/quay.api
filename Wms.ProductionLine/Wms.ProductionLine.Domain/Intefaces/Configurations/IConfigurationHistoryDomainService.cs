using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Wms.ProductionLine.Domain.Dto;
using Wms.ProductionLine.Domain.Dto.Filter;
using Wms.ProductionLine.Domain.Entities;

namespace Wms.ProductionLine.Domain.Intefaces.Configurations
{
    public interface IConfigurationHistoryDomainService
    {
        Task AddAsync(ProductionLineConfiguration configuration, Guid userId, string userLogin);
        Task RegisterChanges(ProductionLineConfigurationDto dto, ProductionLineConfiguration configuration);
        Task<PaginatedResult<ProductionLineConfigurationHistoryDto>> GetHistory(ConfigurationHistoryFilter filter);
    }
}
