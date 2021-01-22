using Hbsis.Wms.Contracts.ProcessConfiguration.ProductionLine;
using Hbsis.Wms.Infra.Domain.Interfaces.Data;
using Hbsis.Wms.Infra.RabbitMq;
using Microsoft.Extensions.Logging;
using NSubstitute;
using System;
using Wms.ProductionLine.Api.Application;
using Wms.ProductionLine.Domain.Dto;
using Wms.ProductionLine.Domain.Entities;
using Wms.ProductionLine.Domain.Intefaces.Configurations;
using Xunit;

namespace Wms.ProductionLine.Tests.Unit.Application.Service
{
    public class ProductionLineConfigurationServiceTests
    {
        private readonly IConfigurationApplicationService _service;
        private readonly IProductionLineConfigurationRepository _repository;
        private readonly IUnitOfWork _uow;
        private readonly IBusPublisher _rabbitMqBus;
        private readonly ILogger<ProductionLineConfiguration> _logger;
        private IConfigurationHistoryDomainService _configurationHistoryDomainService;

        public ProductionLineConfigurationServiceTests()
        {
            _repository = Substitute.For<IProductionLineConfigurationRepository>();
            _uow = Substitute.For<IUnitOfWork>();
            _rabbitMqBus = Substitute.For<IBusPublisher>();
            _logger = Substitute.For<ILogger<ProductionLineConfiguration>>();
            _configurationHistoryDomainService = Substitute.For<IConfigurationHistoryDomainService>();
            _service = new ConfigurationApplicationService(_uow, _repository, _configurationHistoryDomainService, _rabbitMqBus, _logger);
        }

        [Fact]
        public void should_publish_message_when_creating_configuration()
        {
            var configuration = new ProductionLineConfigurationDto
            {
                Enabled = false,
                WarehouseId = Guid.NewGuid()
            };
            var userId = Guid.NewGuid();
            var login = "admin";
            _service.AddConfiguration(configuration, userId, login);
            _repository.Received(1).Add(Arg.Any<ProductionLineConfiguration>());
            _uow.Received(1).Commit();
            _rabbitMqBus.Received(1).Send(Arg.Any<ProductionLineConfigurationEvent>());
        }

        [Fact]
        public void should_publish_message_when_updating_configuration()
        {
            var configurationDto = new ProductionLineConfigurationDto
            {
                Id = Guid.NewGuid(),
                Enabled = false,
                WarehouseId = Guid.NewGuid()
            };

            var configuration = new ProductionLineConfiguration(configurationDto);
            configuration.SetId(configurationDto.Id.Value);
            var userId = Guid.NewGuid();
            var login = "admin";

            _repository.GetByWarehouse(Arg.Any<Guid>()).Returns(configuration);

            _service.UpdateConfiguration(configurationDto, userId, login);
            _uow.Received(1).Commit();
            _rabbitMqBus.Received(1).Send(Arg.Any<ProductionLineConfigurationEvent>());
        }
    }
}
