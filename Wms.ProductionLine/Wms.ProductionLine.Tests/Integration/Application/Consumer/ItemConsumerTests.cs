using AutoMapper;
using FluentAssertions;
using Hbsis.Wms.Contracts.Items;
using Hbsis.Wms.Infra.Data.Repository;
using Hbsis.Wms.Infra.Data.Sqlite;
using Microsoft.Extensions.Logging;
using NSubstitute;
using System;
using System.Threading.Tasks;
using Wms.ProductionLine.Api.Consumers;
using Wms.ProductionLine.Domain.Entities;
using Wms.ProductionLine.Infra.Data.Context;
using Wms.ProductionLine.Infra.Repository;
using Wms.ProductionLine.Tests.AutoMapper;
using Xunit;

namespace Wms.ProductionLine.Tests.Integration.Application.Consumer
{
    public class ItemConsumerTests : SqLiteIntegrationTest<ProductionLineContext>
    {
        private readonly ILogger<ItemConsumer> _logger;
        private readonly IMapper _mapper;

        public ItemConsumerTests()
        {
            _logger = Substitute.For<ILogger<ItemConsumer>>();
            _mapper = AutoMapperFixture.GetInstance();
        }

        [Fact]
        public async Task should_create_item_async()
        {
            //Arrange
            var itemId = Guid.NewGuid();
            var itemCode = "988";
            var itemDescription = "BRAHMA";
            var itemType = 1;

            var evtItemCreated = new ItemCriado
            {
                Id = itemId,
                Codigo = itemCode,
                Descricao = itemDescription,
                TipoItem = itemType
            };

            await ExecuteCommand(async (context) => 
            {
                var unitOfWork = new UnitOfWork(context);
                var itemRepository = new ItemRepository(context);
                var itemConsumer = new ItemConsumer(unitOfWork, _mapper, _logger, itemRepository);

                //Act
                await itemConsumer.Consume(evtItemCreated);
            });

            await ExecuteCommand(async (context) =>
            {
                //Assert
                var itemRepository = new ItemRepository(context);
                var item = itemRepository.GetById(itemId);

                item.Code.Should().Be(itemCode);
                item.Description.Should().Be(itemDescription);
                item.ItemType.Should().Be(itemType);
            });
        }

        [Fact]
        public async Task should_update_item_async()
        {
            //Arrange
            var itemId = Guid.NewGuid();
            var item = new Item(itemId, "BRAHMA", "988", 1);
            await AddEntities(item);

            var evtUpdated = new ItemAtualizado
            {
                Id = item.Id,
                Codigo = "1399",
                Descricao = "SKOL",
                TipoItem = 2
            };

            await ExecuteCommand(async (context) => 
            {
                var unitOfWork = new UnitOfWork(context);
                var itemRepository = new ItemRepository(context);
                var itemConsumer = new ItemConsumer(unitOfWork, _mapper, _logger, itemRepository);

                //Act
                await itemConsumer.Consume(evtUpdated);
            });
                        
            await ExecuteCommand(async (context) =>
            {
                var itemRepository = new ItemRepository(context);
                var itemUpdated = itemRepository.GetById(itemId);

                //Assert
                itemUpdated.Code.Should().Be("1399");
                itemUpdated.Description.Should().Be("SKOL");
                itemUpdated.ItemType.Should().Be(2);
            });
        }

        [Fact]
        public async Task should_create_when_not_found()
        {
            //Arrange
            var itemId = Guid.NewGuid();
            var item = new Item(itemId, "BRAHMA", "988", 1);

            var evtUpdated = new ItemAtualizado
            {
                Id = item.Id,
                Codigo = "1399",
                Descricao = "SKOL",
                TipoItem = 2
            };

            await ExecuteCommand(async (context) =>
            {
                var unitOfWork = new UnitOfWork(context);
                var itemRepository = new ItemRepository(context);
                var itemConsumer = new ItemConsumer(unitOfWork, _mapper, _logger, itemRepository);

                //Act
                await itemConsumer.Consume(evtUpdated);
            });

            await ExecuteCommand(async (context) =>
            {
                var itemRepository = new ItemRepository(context);
                var itemUpdated = itemRepository.GetById(itemId);

                //Assert
                itemUpdated.Code.Should().Be("1399");
                itemUpdated.Description.Should().Be("SKOL");
                itemUpdated.ItemType.Should().Be(2);
            });
        }
    }
}
