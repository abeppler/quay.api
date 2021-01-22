using FluentAssertions;
using Hbsis.Wms.Infra.Domain.Interfaces.Data;
using Hbsis.Wms.Infra.RabbitMq;
using Microsoft.Extensions.Logging;
using NSubstitute;
using System;
using System.Threading.Tasks;
using Wms.ProductionLine.Api.Application;
using Wms.ProductionLine.Domain.Dto;
using Wms.ProductionLine.Domain.Entities;
using Wms.ProductionLine.Domain.Intefaces.Configurations;
using Xunit;

namespace Wms.ProductionLine.Tests.Unit.Application
{
    public class ConfigurationApplicationServiceTest
    {
        private readonly IConfigurationApplicationService _configurationApplicationService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IProductionLineConfigurationRepository _repository;
        private readonly IConfigurationHistoryDomainService _configurationHistoryDomainService;
        private readonly IBusPublisher _rabbitMqBus;
        private readonly ILogger<ProductionLineConfiguration> _logger;

        public ConfigurationApplicationServiceTest()
        {
            _unitOfWork = Substitute.For<IUnitOfWork>();
            _repository = Substitute.For<IProductionLineConfigurationRepository>();
            _rabbitMqBus = Substitute.For<IBusPublisher>();
            _logger = Substitute.For<ILogger<ProductionLineConfiguration>>();
            _configurationHistoryDomainService = Substitute.For<IConfigurationHistoryDomainService>();
            _configurationApplicationService = new ConfigurationApplicationService(_unitOfWork, _repository, _configurationHistoryDomainService, _rabbitMqBus, _logger);
        }

        [Fact]
        public async Task GetConfiguration_should_get_configuration_for_provided_warehouse()
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

            _repository.GetByWarehouse(Arg.Is<Guid>(x => x == warehouseId)).Returns(configuration);

            //Act
            var result = _configurationApplicationService.GetConfiguration(warehouseId);

            //Assert
            result.Enabled.Should().BeTrue();
            result.WarehouseId.Should().Be(warehouseId);
            result.Id.Should().Be(configId);
        }

        [Fact]
        public async Task AddConfiguration_should_add_new_configuration()
        {
            //Arrange
            var warehouseId = Guid.NewGuid();
            var configuration = new ProductionLineConfigurationDto()
            {
                Enabled = true,
                WarehouseId = warehouseId
            };
            var userId = Guid.NewGuid();
            var userLogin = "master";

            //Act
            await _configurationApplicationService.AddConfiguration(configuration, userId, userLogin);

            //Assert
            _repository.Received(1).Add(Arg.Is<ProductionLineConfiguration>(x =>
                x.Enabled == true
                && x.WarehouseId == warehouseId));

            await _configurationHistoryDomainService.Received(1).AddAsync(Arg.Is<ProductionLineConfiguration>(x =>
                x.Enabled == true
                && x.WarehouseId == warehouseId), userId, userLogin);

            await _unitOfWork.Received(1).Commit();
        }

        [Fact]
        public async Task UpdateConfiguration_should_alter_configuration()
        {
            //Arrange
            var userId = Guid.NewGuid();
            var userLogin = "master";

            var warehouseId = Guid.NewGuid();
            var configuration = new ProductionLineConfiguration(new ProductionLineConfigurationDto()
            {
                Enabled = true,
                WarehouseId = warehouseId
            });

            var configId = Guid.NewGuid();
            configuration.SetId(configId);

            _repository.GetByWarehouse(warehouseId).Returns(configuration);

            //Act
            var configToUpdate = new ProductionLineConfigurationDto()
            {
                Id = configId,
                Enabled = false,
                WarehouseId = warehouseId
            };

            await _configurationApplicationService.UpdateConfiguration(configToUpdate, userId, userLogin);

            //Assert
            await _configurationHistoryDomainService.Received(1).RegisterChanges(configToUpdate, configuration);
            await _unitOfWork.Received(1).Commit();

            configuration.Enabled.Should().BeFalse();
        }
    }
}
