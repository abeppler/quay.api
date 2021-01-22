using System;
using System.Threading.Tasks;
using Wms.ProductionLine.Domain.Dto;

namespace Wms.ProductionLine.Api.Application
{
    public interface IConfigurationApplicationService
    {
        ProductionLineConfigurationDto GetConfiguration(Guid warehouseId);
        Task AddConfiguration(ProductionLineConfigurationDto configurationDto, Guid userId, string userLogin);
        Task UpdateConfiguration(ProductionLineConfigurationDto configurationDto, Guid userId, string userLogin);
    }
}
