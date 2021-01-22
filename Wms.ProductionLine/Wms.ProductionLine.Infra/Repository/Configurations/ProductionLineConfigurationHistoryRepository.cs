using System.Threading.Tasks;
using Hbsis.Wms.Infra.Data.Repository;
using Microsoft.EntityFrameworkCore;
using Wms.ProductionLine.Domain.Dto;
using Wms.ProductionLine.Domain.Dto.Filter;
using Wms.ProductionLine.Domain.Entities.History;
using Wms.ProductionLine.Domain.Intefaces.Configurations;
using System.Linq;
using Wms.ProductionLine.CrossCutting;

namespace Wms.ProductionLine.Infra.Repository.Configurations
{
    public class ProductionLineConfigurationHistoryRepository : Repository<ProductionLineConfigurationHistory>, IProductionLineConfigurationHistoryRepository
    {
        public ProductionLineConfigurationHistoryRepository(DbContext context) : base(context)
        {
        }

        public async Task<PaginatedResult<ProductionLineConfigurationHistoryDto>> GetHistory(ConfigurationHistoryFilter filter)
        {
            var query = _set.AsNoTracking().Where(x => x.ProductionLineConfigurationId == filter.ConfigurationId);
            var count = await query.CountAsync();

            var skip = (filter.PageIndex - 1) * filter.ItemsByPage;
            var history = await query.OrderByDescending(x => x.CreatedDate)
                .Skip(skip)
                .Take(filter.ItemsByPage)
                .ToListAsync();

            var dto = history.Select(x => new ProductionLineConfigurationHistoryDto
            {
                UserId = x.UserId,
                HistoryType = new TypeHistoryDto { Id = (int)x.HistoryType, Label = x.HistoryType.GetDisplayName() },
                Description = x.Description,
                CreatedDate = x.CreatedDate
            }).ToList();

            return new PaginatedResult<ProductionLineConfigurationHistoryDto>(count, dto);
        }
    }
}
