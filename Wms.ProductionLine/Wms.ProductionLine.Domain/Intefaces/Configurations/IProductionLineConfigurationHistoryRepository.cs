using System.Threading.Tasks;
using Hbsis.Wms.Infra.Domain.Interfaces.Data;
using Wms.ProductionLine.Domain.Dto;
using Wms.ProductionLine.Domain.Dto.Filter;
using Wms.ProductionLine.Domain.Entities.History;

namespace Wms.ProductionLine.Domain.Intefaces.Configurations
{
    public interface IProductionLineConfigurationHistoryRepository : IRepository<ProductionLineConfigurationHistory>, IAsyncRepository<ProductionLineConfigurationHistory>
    {
        Task<PaginatedResult<ProductionLineConfigurationHistoryDto>> GetHistory(ConfigurationHistoryFilter filter);
    }
}
