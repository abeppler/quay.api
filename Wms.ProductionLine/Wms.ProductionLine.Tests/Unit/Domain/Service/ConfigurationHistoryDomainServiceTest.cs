using NSubstitute;
using System;
using System.Threading.Tasks;
using Wms.ProductionLine.Domain.Dto;
using Wms.ProductionLine.Domain.Entities;
using Wms.ProductionLine.Domain.Entities.History;
using Wms.ProductionLine.Domain.Intefaces.Configurations;
using Wms.ProductionLine.Domain.Services.Configurations;
using Xunit;

namespace Wms.ProductionLine.Tests.Unit.Domain.Service
{
    public class ConfigurationHistoryDomainServiceTest
    {
        private readonly IConfigurationHistoryDomainService _configurationHistoryDomainService;
        private readonly IProductionLineConfigurationHistoryRepository _configurationHistoryRepository;

        public ConfigurationHistoryDomainServiceTest()
        {
            _configurationHistoryRepository = Substitute.For<IProductionLineConfigurationHistoryRepository>();
            _configurationHistoryDomainService = new ConfigurationHistoryDomainService(_configurationHistoryRepository);
        }

        [Fact]
        public async Task AddAsync_should_register_new_configuration()
        {
            //Arrange
            var warehouseId = Guid.NewGuid();
            var configuration = new ProductionLineConfiguration(new ProductionLineConfigurationDto()
            {
                Enabled = true,
                WarehouseId = warehouseId
            });

            var configId = Guid.NewGuid();
            configuration.SetId(configId);

            var userId = Guid.NewGuid();

            //Act
            await _configurationHistoryDomainService.AddAsync(configuration, userId, "master");

            //Assert
            await _configurationHistoryRepository.Received(1).AddAsync(Arg.Is<ProductionLineConfigurationHistory>(x =>
                x.HistoryType == ConfigurationHistoryType.NewConfiguration
                && x.UserId == userId
                && x.ProductionLineConfigurationId == configId
            ));
        }

        [Fact]
        public async Task RegisterChanges_should_register_status_change()
        {
            //Arrange
            var warehouseId = Guid.NewGuid();
            var configuration = new ProductionLineConfiguration(new ProductionLineConfigurationDto()
            {
                Enabled = true,
                WarehouseId = warehouseId
            });

            var configId = Guid.NewGuid();
            configuration.SetId(configId);

            var userId = Guid.NewGuid();

            var configurationToUpdate = new ProductionLineConfigurationDto()
            {
                Id = configId,
                Enabled = false,
                WarehouseId = warehouseId,
                UserId = userId
            };            

            //Act
            await _configurationHistoryDomainService.RegisterChanges(configurationToUpdate, configuration);

            //Assert
            await _configurationHistoryRepository.Received(1).AddAsync(Arg.Is<ProductionLineConfigurationHistory>(x =>
                x.HistoryType == ConfigurationHistoryType.StatusChanged
                && x.UserId == userId
                && x.ProductionLineConfigurationId == configId
            ));
        }
    }
}
