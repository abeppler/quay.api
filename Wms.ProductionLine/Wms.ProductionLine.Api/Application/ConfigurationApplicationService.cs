using Hbsis.Wms.Contracts.ProcessConfiguration.ProductionLine;
using Hbsis.Wms.Infra.Domain.Interfaces.Data;
using Hbsis.Wms.Infra.RabbitMq;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using Wms.ProductionLine.Domain.Dto;
using Wms.ProductionLine.Domain.Dto.Filter;
using Wms.ProductionLine.Domain.Entities;
using Wms.ProductionLine.Domain.Intefaces.Configurations;

namespace Wms.ProductionLine.Api.Application
{
    public class ConfigurationApplicationService : IConfigurationApplicationService
    {
        private IProductionLineConfigurationRepository _repository;
        private IUnitOfWork _uow;
        private IBusPublisher _rabbitMqBus;
        private readonly ILogger<ProductionLineConfiguration> _logger;
        private IConfigurationHistoryDomainService _configurationHistoryDomainService;

        public ConfigurationApplicationService(
            IUnitOfWork uow, 
            IProductionLineConfigurationRepository repository,
            IConfigurationHistoryDomainService configurationHistoryDomainService,
            IBusPublisher rabbitMqBus,
            ILogger<ProductionLineConfiguration> logger)
        {
            _repository = repository;
            _uow = uow;
            _rabbitMqBus = rabbitMqBus;
            _logger = logger;
            _configurationHistoryDomainService = configurationHistoryDomainService;
        }

        public async Task AddConfiguration(ProductionLineConfigurationDto configurationDto, Guid userId, string userLogin)
        {
            var newConfiguration = new ProductionLineConfiguration(configurationDto);
            _repository.Add(newConfiguration);
            await _configurationHistoryDomainService.AddAsync(newConfiguration, userId, userLogin);
            await _uow.Commit();
            _logger.LogDebug($"Creating ProductLineConfiguration of Warehouse: {newConfiguration.WarehouseId}");

            PublishConfigurationEvent(newConfiguration);
        }

        public async Task UpdateConfiguration(ProductionLineConfigurationDto configurationDto, Guid userId, string userLogin)
        {
            var configuration = _repository.GetByWarehouse(configurationDto.WarehouseId.Value);

            configurationDto.UserLogin = userLogin;
            configurationDto.UserId = userId;
            await _configurationHistoryDomainService.RegisterChanges(configurationDto, configuration);

            configuration.UpdateProperties(configurationDto);

            await _uow.Commit();
            _logger.LogDebug($"Updating ProductLineConfiguration of Warehouse: {configuration.WarehouseId}");

            PublishConfigurationEvent(configuration);
        }

        public ProductionLineConfigurationDto GetConfiguration(Guid warehouseId)
        {
            var configuration = _repository.GetByWarehouse(warehouseId);
            return configuration == null ? null : new ProductionLineConfigurationDto(configuration);
        }

        private async void PublishConfigurationEvent(ProductionLineConfiguration newConfiguration)
        {
            var @event = new ProductionLineConfigurationEvent
            {
                WarehouseId = newConfiguration.WarehouseId,
                Enabled = newConfiguration.Enabled,
            };

            _logger.LogDebug($"Publishing 'ConfiguracaoProcessoAtualizada'=> Warehouse:{@event.WarehouseId} Process: line.configuration");
            await _rabbitMqBus.Send(@event);
            _logger.LogDebug($"Published 'ConfiguracaoProcessoAtualizada'=> Warehouse:{@event.WarehouseId} Process: line.configuration");
        }
    }
}
