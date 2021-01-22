using FluentAssertions;
using Hbsis.Wms.Contracts.Addresses;
using Hbsis.Wms.Infra.Data.Repository;
using Hbsis.Wms.Infra.Data.Sqlite;
using Microsoft.Extensions.Logging;
using NSubstitute;
using System;
using System.Threading.Tasks;
using Wms.ProductionLine.Api.Consumers;
using Wms.ProductionLine.Domain.Dto;
using Wms.ProductionLine.Domain.Entities;
using Wms.ProductionLine.Infra.Data.Context;
using Wms.ProductionLine.Infra.Repository;
using Xunit;

namespace Wms.ProductionLine.Tests.Integration.Application.Consumer
{
    public class AddressConsumerTests : SqLiteIntegrationTest<ProductionLineContext>
    {
        private readonly ILogger<AddressConsumer> _logger;

        public AddressConsumerTests()
        {
            _logger = Substitute.For<ILogger<AddressConsumer>>();
        }

        [Fact]
        public async Task should_create_address()
        {
            var eventCreated = new EnderecoCriado
            {
                Id = Guid.NewGuid(),
                IdArmazem = Guid.NewGuid(),
                Codigo = "A03",
                Descricao = "Armazenagem PA",
                Enabled = true,
                Sequence = 1
            };

            await ExecuteCommand(async (context) =>
            {
                var AddressRepository = new AddressRepository(context);
                var unitOfWork = new UnitOfWork(context);
                var addressConsumer = new AddressConsumer(unitOfWork, AddressRepository, _logger);

                await addressConsumer.Consume(eventCreated);

                var address = AddressRepository.GetById(eventCreated.Id);
                address.WarehouseId.Should().Be(eventCreated.IdArmazem);
                address.Code.Should().Be(eventCreated.Codigo);
                address.Description.Should().Be(eventCreated.Descricao);
                address.Enabled.Should().Be(eventCreated.Enabled);
                address.Sequence.Should().Be(eventCreated.Sequence);
            });
        }

        [Fact]
        public async Task should_update_address()
        {
            var addressDto = new AddressDto
            {
                Id = Guid.NewGuid(),
                WarehouseId = Guid.NewGuid(),
                Code = "A04",
                Description = "Armazenagem PA04",
                Enabled = true,
                Sequence = 1
            };

            var address = new Address(addressDto);

            await ExecuteCommand(async (context) =>
            {
                var addressRepository = new AddressRepository(context);
                var unitOfWork = new UnitOfWork(context);
                await addressRepository.AddAsync(address);
                await unitOfWork.Commit();
            });

            var eventUpdated = new EnderecoAtualizado
            {
                Id = address.Id,
                IdArmazem = address.WarehouseId,
                Codigo = "A04_NOVO",
                Descricao = "Armazenagem PA_NOVO",
                Enabled = false,
                Sequence = 2
            };

            await ExecuteCommand(async (context) =>
            {
                var addressRepository = new AddressRepository(context);
                var unitOfWork = new UnitOfWork(context);
                var addressConsumer = new AddressConsumer(unitOfWork, addressRepository, _logger);

                await addressConsumer.Consume(eventUpdated);

                var addressUpdated = addressRepository.GetById(eventUpdated.Id);
                addressUpdated.WarehouseId.Should().Be(eventUpdated.IdArmazem);
                addressUpdated.Code.Should().Be(eventUpdated.Codigo);
                addressUpdated.Description.Should().Be(eventUpdated.Descricao);
                addressUpdated.Enabled.Should().Be(eventUpdated.Enabled);
                addressUpdated.Sequence.Should().Be(eventUpdated.Sequence);
            });
        }

        [Fact]
        public async Task should_create_when_address_not_found()
        {
            var eventUpdated = new EnderecoAtualizado
            {
                Id = Guid.NewGuid(),
                IdArmazem = Guid.NewGuid(),
                Codigo = "A04_NOVO",
                Descricao = "Armazenagem PA_NOVO",
                Enabled = false,
                Sequence = 2
            };

            await ExecuteCommand(async (context) =>
            {
                var addressRepository = new AddressRepository(context);
                var unitOfWork = new UnitOfWork(context);
                var addressConsumer = new AddressConsumer(unitOfWork, addressRepository, _logger);

                await addressConsumer.Consume(eventUpdated);

                var addressUpdated = addressRepository.GetById(eventUpdated.Id);
                addressUpdated.WarehouseId.Should().Be(eventUpdated.IdArmazem);
                addressUpdated.Code.Should().Be(eventUpdated.Codigo);
                addressUpdated.Description.Should().Be(eventUpdated.Descricao);
                addressUpdated.Enabled.Should().Be(eventUpdated.Enabled);
                addressUpdated.Sequence.Should().Be(eventUpdated.Sequence);
            });
        }
    }
}
