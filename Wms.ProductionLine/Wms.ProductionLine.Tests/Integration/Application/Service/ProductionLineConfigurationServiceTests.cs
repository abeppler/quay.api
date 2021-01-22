using FluentAssertions;
using Hbsis.Wms.Contracts.ProcessConfiguration.ProductionLine;
using Hbsis.Wms.Infra.Data.Repository;
using Hbsis.Wms.Infra.Data.Sqlite;
using Hbsis.Wms.Infra.RabbitMq;
using Microsoft.Extensions.Logging;
using NSubstitute;
using System;
using System.Threading.Tasks;
using Wms.ProductionLine.Api.Application;
using Wms.ProductionLine.Domain.Dto;
using Wms.ProductionLine.Domain.Entities;
using Wms.ProductionLine.Domain.Services.Configurations;
using Wms.ProductionLine.Infra.Data.Context;
using Wms.ProductionLine.Infra.Repository.Configurations;
using Xunit;

namespace Wms.ProductionLine.Tests.Integration.Application.Service
{
    public class ProductionLineConfigurationServiceTests : SqLiteIntegrationTest<ProductionLineContext>
    {
        private readonly IBusPublisher _rabbitMqBus;
        private readonly ILogger<ProductionLineConfiguration> _logger;

        public ProductionLineConfigurationServiceTests()
        {
            _rabbitMqBus = Substitute.For<IBusPublisher>();
            _logger = Substitute.For<ILogger<ProductionLineConfiguration>>();
        }

        [Fact]
        public async Task should_add_configuration()
        {
            var configurationDto = new ProductionLineConfigurationDto
            {
                Enabled = false,
                WarehouseId = Guid.NewGuid()
            };

            await ExecuteCommand(async (context) =>
            {
                var configurationRepository = new ProductionLineConfigurationRepository(context);
                var unitOfWork = new UnitOfWork(context);
                var configurationHistoryRepository = new ProductionLineConfigurationHistoryRepository(context);
                var configurationHistoryDomainService = new ConfigurationHistoryDomainService(configurationHistoryRepository);
                var configurationService = new ConfigurationApplicationService(
                    unitOfWork,
                    configurationRepository, 
                    configurationHistoryDomainService, 
                    _rabbitMqBus, 
                    _logger);

                await configurationService.AddConfiguration(configurationDto, Guid.NewGuid(), "admin");
                await _rabbitMqBus.Received(1).Send(Arg.Any<ProductionLineConfigurationEvent>());
            });

            var configuration = await ExecuteCommand(async (context) =>
            {
                var configurationRepository = new ProductionLineConfigurationRepository(context);
                var unitOfWork = new UnitOfWork(context);
                var configurationHistoryRepository = new ProductionLineConfigurationHistoryRepository(context);
                var configurationHistoryDomainService = new ConfigurationHistoryDomainService(configurationHistoryRepository);
                var configurationService = new ConfigurationApplicationService(
                    unitOfWork,
                    configurationRepository,
                    configurationHistoryDomainService,
                    _rabbitMqBus,
                    _logger);

                return configurationService.GetConfiguration(configurationDto.WarehouseId.Value);
            });

            configuration.WarehouseId.Should().Be(configurationDto.WarehouseId);
            configuration.Enabled.Should().Be(configurationDto.Enabled);
        }

        [Fact]
        public async Task should_update_configuration()
        {
            var createConfigurationDto = new ProductionLineConfigurationDto
            {
                Enabled = false,
                WarehouseId = Guid.NewGuid()
            };

            var configuration = await ExecuteCommand(async (context) =>
            {
                var configurationRepository = new ProductionLineConfigurationRepository(context);
                var unitOfWork = new UnitOfWork(context);
                var configurationHistoryRepository = new ProductionLineConfigurationHistoryRepository(context);
                var configurationHistoryDomainService = new ConfigurationHistoryDomainService(configurationHistoryRepository);
                var configurationService = new ConfigurationApplicationService(
                    unitOfWork,
                    configurationRepository,
                    configurationHistoryDomainService,
                    _rabbitMqBus,
                    _logger);

                await configurationService.AddConfiguration(createConfigurationDto, Guid.NewGuid(), "admin");
                await _rabbitMqBus.Received(1).Send(Arg.Any<ProductionLineConfigurationEvent>());
                return configurationService.GetConfiguration(createConfigurationDto.WarehouseId.Value);
            });

            configuration.Enabled = true;

            var updatedConfiguration = await ExecuteCommand(async (context) =>
            {
                var configurationRepository = new ProductionLineConfigurationRepository(context);
                var unitOfWork = new UnitOfWork(context);
                var configurationHistoryRepository = new ProductionLineConfigurationHistoryRepository(context);
                var configurationHistoryDomainService = new ConfigurationHistoryDomainService(configurationHistoryRepository);
                var configurationService = new ConfigurationApplicationService(
                    unitOfWork,
                    configurationRepository,
                    configurationHistoryDomainService,
                    _rabbitMqBus,
                    _logger);

                await configurationService.UpdateConfiguration(configuration, Guid.NewGuid(), "admin");
                await _rabbitMqBus.Received(2).Send(Arg.Any<ProductionLineConfigurationEvent>());
                return configurationService.GetConfiguration(createConfigurationDto.WarehouseId.Value);
            });

            updatedConfiguration.WarehouseId.Should().Be(configuration.WarehouseId);
            updatedConfiguration.Enabled.Should().Be(configuration.Enabled);
        }
    }
}
