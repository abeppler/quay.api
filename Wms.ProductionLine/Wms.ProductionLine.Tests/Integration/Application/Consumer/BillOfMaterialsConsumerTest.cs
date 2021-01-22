using AutoMapper;
using FluentAssertions;
using Hbsis.Wms.Contracts.BillsOfMaterials;
using Hbsis.Wms.Infra.Data.Repository;
using Hbsis.Wms.Infra.Data.Sqlite;
using Microsoft.Extensions.Logging;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Wms.ProductionLine.Api.Consumers;
using Wms.ProductionLine.Domain.Entities;
using Wms.ProductionLine.Infra.Data.Context;
using Wms.ProductionLine.Infra.Repository;
using Wms.ProductionLine.Tests.AutoMapper;
using Wms.ProductionLine.Tests.Builders;
using Xunit;

namespace Wms.ProductionLine.Tests.Integration.Application.Consumer
{
    public class BillOfMaterialsConsumerTest : SqLiteIntegrationTest<ProductionLineContext>
    {
        private readonly ILogger<BillOfMaterialsConsumer> _logger;
        private readonly IMapper _mapper;

        public BillOfMaterialsConsumerTest()
        {
            _logger = Substitute.For<ILogger<BillOfMaterialsConsumer>>();
            _mapper = AutoMapperFixture.GetInstance();
        }

        [Fact]
        public async Task Consumer_CreatedBillOfMaterials_ShouldCreateIntegralizado()
        {
            //Arrange
            var integralizadoId = Guid.NewGuid();
            var billItemHeader = (new ItemBuilder().WithDescription("Kit").Build());
            var billItemDetail1 = (new ItemBuilder().WithDescription("item1").Build());
            var billItemDetail2 = (new ItemBuilder().WithDescription("item2").Build());
            await AddEntities(billItemHeader, billItemDetail1, billItemDetail2);

            var evtCreatedBillOfMaterials = new CreatedBillOfMaterials
            {
                Enabled = true,
                Id = integralizadoId,
                Item = billItemHeader.Id,
                WarehouseId = Guid.NewGuid(),
                Details = new List<EventIntegralizadoDetail>
                {
                    new EventIntegralizadoDetail { Id = Guid.NewGuid(), ItemId = billItemDetail1.Id, Quantity = 10 },
                    new EventIntegralizadoDetail { Id = Guid.NewGuid(), ItemId = billItemDetail2.Id, Quantity = 20 }
                }
            };

            //Act
            await ExecuteCommand(async (context) => 
            {
                var integralizadoRepository = new IntegralizadoRepository(context);
                var integralizadoDetailRepository = new IntegralizadoDetailRepository(context);
                var unitOfWork = new UnitOfWork(context);
                var billOfMaterialsConsumer = 
                    new BillOfMaterialsConsumer(unitOfWork, _mapper, integralizadoRepository, integralizadoDetailRepository, _logger);

                await billOfMaterialsConsumer.Consume(evtCreatedBillOfMaterials);
            });

            //Assert
            await ExecuteCommand(async (context) =>
            {
                var integralizadoRepository = new IntegralizadoRepository(context);

                var integralizado = await integralizadoRepository.GetWithDetails(integralizadoId);

                integralizado.ItemId.Should().Be(billItemHeader.Id);
                integralizado.Enabled.Should().BeTrue();

                integralizado.Details.Should().HaveCount(2);

                var items = integralizado.Details.ToArray();
                items[0].ItemId.Should().Be(billItemDetail1.Id);
                items[0].Quantity.Should().Be(10);

                items[1].ItemId.Should().Be(billItemDetail2.Id);
                items[1].Quantity.Should().Be(20);
            });
        }

        [Fact]
        public async Task Consumer_AlteredBillOfMaterials_ShouldUpdateIntegralizado()
        {
            //Arrange
            var warehouseId = Guid.NewGuid();
            var integralizadoId = Guid.NewGuid();
            var billItemHeader = (new ItemBuilder().WithDescription("Kit").Build());
            var billItemDetail1 = (new ItemBuilder().WithDescription("item1").Build());
            var billItemDetail2 = (new ItemBuilder().WithDescription("item2").Build());
            var billItemDetail3 = (new ItemBuilder().WithDescription("item3").Build());

            var integralizadoDetails = new List<IntegralizadoDetail>
            {
                new IntegralizadoDetail(Guid.NewGuid(), billItemDetail1.Id, 10),
                new IntegralizadoDetail(Guid.NewGuid(), billItemDetail2.Id, 20)
            };

            var integralizado = new Integralizado(integralizadoId, billItemHeader.Id, true, warehouseId, integralizadoDetails);

            await AddEntities(billItemHeader, billItemDetail1, billItemDetail2,billItemDetail3, integralizado);

            var evtAlteredBillOfMaterials = new AlteredBillOfMaterials
            {
                Enabled = true,
                Id = integralizadoId,
                Item = billItemHeader.Id,
                WarehouseId = warehouseId,
                Details = new List<EventIntegralizadoDetail>
                {
                    new EventIntegralizadoDetail { Id = Guid.NewGuid(), ItemId = billItemDetail3.Id, Quantity = 30 }
                }
            };

            //Act
            await ExecuteCommand(async (context) =>
            {
                var integralizadoRepository = new IntegralizadoRepository(context);
                var integralizadoDetailRepository = new IntegralizadoDetailRepository(context);
                var unitOfWork = new UnitOfWork(context);
                var billOfMaterialsConsumer = new BillOfMaterialsConsumer(unitOfWork, _mapper, integralizadoRepository, 
                    integralizadoDetailRepository, _logger);

                await billOfMaterialsConsumer.Consume(evtAlteredBillOfMaterials);
            });

            //Assert
            await ExecuteCommand(async (context) =>
            {
                var integralizadoRepository = new IntegralizadoRepository(context);
                var integralizadoUpdated = await integralizadoRepository.GetWithDetails(integralizadoId);

                integralizadoUpdated.ItemId.Should().Be(billItemHeader.Id);
                integralizadoUpdated.Enabled.Should().BeTrue();
                integralizadoUpdated.Details.Should().HaveCount(1);

                var items = integralizadoUpdated.Details.ToArray();

                items[0].ItemId.Should().Be(billItemDetail3.Id);
                items[0].Quantity.Should().Be(30);
            });
        }

        [Fact]
        public async Task Consumer_AlteredBillOfMaterials_ShouldCreateIntegralizadoWhenNotFound()
        {
            //Arrange
            var warehouseId = Guid.NewGuid();
            var integralizadoId = Guid.NewGuid();
            var billItemHeader = (new ItemBuilder().WithDescription("Kit").Build());
            var billItemDetail1 = (new ItemBuilder().WithDescription("item1").Build());
            var billItemDetail2 = (new ItemBuilder().WithDescription("item2").Build());
            var billItemDetail3 = (new ItemBuilder().WithDescription("item3").Build());

            await AddEntities(billItemHeader, billItemDetail1, billItemDetail2, billItemDetail3);

            var evtAlteredBillOfMaterials = new AlteredBillOfMaterials
            {
                Enabled = true,
                Id = integralizadoId,
                Item = billItemHeader.Id,
                WarehouseId = warehouseId,
                Details = new List<EventIntegralizadoDetail>
                {
                    new EventIntegralizadoDetail { Id = Guid.NewGuid(), ItemId = billItemDetail3.Id, Quantity = 30 }
                }
            };

            //Act
            await ExecuteCommand(async (context) =>
            {
                var integralizadoRepository = new IntegralizadoRepository(context);
                var integralizadoDetailRepository = new IntegralizadoDetailRepository(context);
                var unitOfWork = new UnitOfWork(context);
                var billOfMaterialsConsumer = 
                    new BillOfMaterialsConsumer(unitOfWork, _mapper, integralizadoRepository, integralizadoDetailRepository, _logger);

                await billOfMaterialsConsumer.Consume(evtAlteredBillOfMaterials);
            });

            //Assert
            await ExecuteCommand(async (context) =>
            {
                var integralizadoRepository = new IntegralizadoRepository(context);
                var integralizadoUpdated = await integralizadoRepository.GetWithDetails(integralizadoId);

                integralizadoUpdated.ItemId.Should().Be(billItemHeader.Id);
                integralizadoUpdated.Enabled.Should().BeTrue();
                integralizadoUpdated.Details.Should().HaveCount(1);

                var items = integralizadoUpdated.Details.ToArray();

                items[0].ItemId.Should().Be(billItemDetail3.Id);
                items[0].Quantity.Should().Be(30);
            });
        }

        [Fact]
        public async Task Consumer_DeletedBillOfMaterials_ShouldDeleteIntegralizado()
        {
            //Arrange
            var warehouseId = Guid.NewGuid();
            var integralizadoId = Guid.NewGuid();
            var billItemHeader = (new ItemBuilder().WithDescription("Kit").Build());
            var billItemDetail1 = (new ItemBuilder().WithDescription("item1").Build());
            var billItemDetail2 = (new ItemBuilder().WithDescription("item2").Build());
            var integralizadoDetails = new List<IntegralizadoDetail>
            {
                new IntegralizadoDetail(Guid.NewGuid(), billItemDetail1.Id, 10),
                new IntegralizadoDetail(Guid.NewGuid(), billItemDetail2.Id, 20)
            };

            var integralizado = new Integralizado(integralizadoId, billItemHeader.Id, true, warehouseId, integralizadoDetails);

            await AddEntities(billItemHeader, billItemDetail1, billItemDetail2, integralizado);

            var evtDeletedBillOfMaterials = new DeletedBillOfMaterials
            {
                Enabled = true,
                Id = integralizadoId,
                Item = billItemHeader.Id,
                WarehouseId = warehouseId,
                Details = new List<EventIntegralizadoDetail>
                {
                    new EventIntegralizadoDetail { Id = Guid.NewGuid(), ItemId = billItemDetail1.Id, Quantity = 10 },
                    new EventIntegralizadoDetail { Id = Guid.NewGuid(), ItemId = billItemDetail2.Id, Quantity = 20 }
                }
            };

            //Act
            await ExecuteCommand(async (context) =>
            {
                var integralizadoRepository = new IntegralizadoRepository(context);
                var integralizadoDetailRepository = new IntegralizadoDetailRepository(context);
                var unitOfWork = new UnitOfWork(context);
                var billOfMaterialsConsumer = 
                    new BillOfMaterialsConsumer(unitOfWork, _mapper, integralizadoRepository, integralizadoDetailRepository, _logger);

                await billOfMaterialsConsumer.Consume(evtDeletedBillOfMaterials);
            });

            //Assert
            await ExecuteCommand(async (context) =>
            {
                var integralizadoRepository = new IntegralizadoRepository(context);
                var integralizadoDetailRepository = new IntegralizadoDetailRepository(context);

                var integralizadoDeleted = await integralizadoRepository.GetWithDetails(integralizadoId);
                var integralizadoDetailsDeleted = await integralizadoDetailRepository.GetByIntegralizadoId(integralizadoId);

                integralizadoDeleted.Should().BeNull();
                integralizadoDetailsDeleted.Should().BeEmpty();
            });
        }

        [Fact]
        public async Task Consumer_DeletedBillOfMaterials_ShouldNotThrowExceptionWhenIntegralizadoDoesNotExist()
        {
            //Arrange
            var warehouseId = Guid.NewGuid();
            var integralizadoId = Guid.NewGuid();
            var evtDeletedBillOfMaterials = new DeletedBillOfMaterials
            {
                Enabled = true,
                Id = integralizadoId,
                Item = Guid.NewGuid(),
                WarehouseId = warehouseId,
                Details = new List<EventIntegralizadoDetail>
                {
                    new EventIntegralizadoDetail { Id = Guid.NewGuid(), ItemId = Guid.NewGuid(), Quantity = 10 },
                    new EventIntegralizadoDetail { Id = Guid.NewGuid(), ItemId = Guid.NewGuid(), Quantity = 20 }
                }
            };

            //Act
            await ExecuteCommand(async (context) =>
            {
                var integralizadoRepository = new IntegralizadoRepository(context);
                var integralizadoDetailRepository = new IntegralizadoDetailRepository(context);
                var unitOfWork = new UnitOfWork(context);
                var billOfMaterialsConsumer =
                    new BillOfMaterialsConsumer(unitOfWork, _mapper, integralizadoRepository, integralizadoDetailRepository, _logger);

                await billOfMaterialsConsumer.Consume(evtDeletedBillOfMaterials);
            });

            //Assert
            await ExecuteCommand(async (context) =>
            {
                var integralizadoRepository = new IntegralizadoRepository(context);
                var integralizadoDetailRepository = new IntegralizadoDetailRepository(context);

                var integralizadoDeleted = await integralizadoRepository.GetWithDetails(integralizadoId);
                var integralizadoDetailsDeleted = await integralizadoDetailRepository.GetByIntegralizadoId(integralizadoId);

                integralizadoDeleted.Should().BeNull();
                integralizadoDetailsDeleted.Should().BeEmpty();
            });
        }
    }
}
