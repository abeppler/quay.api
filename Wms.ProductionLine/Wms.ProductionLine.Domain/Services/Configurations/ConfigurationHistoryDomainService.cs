using System;
using System.Threading.Tasks;
using Wms.ProductionLine.Domain.Dto;
using Wms.ProductionLine.Domain.Dto.Filter;
using Wms.ProductionLine.Domain.Entities;
using Wms.ProductionLine.Domain.Entities.History;
using Wms.ProductionLine.Domain.Intefaces.Configurations;
using Wms.ProductionLine.Globalization;

namespace Wms.ProductionLine.Domain.Services.Configurations
{
    public class ConfigurationHistoryDomainService : IConfigurationHistoryDomainService
    {
        private readonly IProductionLineConfigurationHistoryRepository _configurationHistoryRepository;
        public ConfigurationHistoryDomainService(IProductionLineConfigurationHistoryRepository configurationHistoryRepository)
        {
            _configurationHistoryRepository = configurationHistoryRepository;
        }

        public Task AddAsync(ProductionLineConfiguration configuration, Guid userId, string userLogin)
        {
            var enabledDescription = configuration.Enabled ? Resources.ConfigurationWasEnabled : Resources.ConfigurationWasDisabled;
            var description = string.Format(enabledDescription, userLogin);

            var history = new ProductionLineConfigurationHistory(userId: userId,
                description: description,
                configurationId: configuration.Id,
                historyType: ConfigurationHistoryType.NewConfiguration,
                createdDate: DateTime.Now);

            return _configurationHistoryRepository.AddAsync(history);
        }

        public Task<PaginatedResult<ProductionLineConfigurationHistoryDto>> GetHistory(ConfigurationHistoryFilter filter)
        {
            return _configurationHistoryRepository.GetHistory(filter);
        }

        public async Task RegisterChanges(ProductionLineConfigurationDto dto, ProductionLineConfiguration configuration)
        {
            if (dto.Enabled != configuration.Enabled)
            {
                await RegisterEnabledChange(dto, configuration);
            }
        }

        private Task RegisterEnabledChange(ProductionLineConfigurationDto dto, ProductionLineConfiguration configuration)
        {
            var description = string.Format(dto.Enabled ? Resources.ConfigurationWasEnabled : Resources.ConfigurationWasDisabled, dto.UserLogin);
            var history = new ProductionLineConfigurationHistory(userId: dto.UserId,
                description: description,
                configurationId: configuration.Id,
                historyType: ConfigurationHistoryType.StatusChanged,
                createdDate: DateTime.Now);

            return _configurationHistoryRepository.AddAsync(history);
        }
    }
}
