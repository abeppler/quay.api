using FluentAssertions;
using Hbsis.Wms.Contracts.Users;
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
using Xunit;

namespace Wms.ProductionLine.Tests.Integration.Application.Consumer
{
    public class UserConsumerTests : SqLiteIntegrationTest<ProductionLineContext>
    {
        private readonly ILogger<UserConsumer> _logger;
        public UserConsumerTests()
        {
            _logger = Substitute.For<ILogger<UserConsumer>>();
        }

        [Fact]
        public async Task should_create_userAsync()
        {
            var eventCreated = new UserCreatedEvent
            {
                Login = "admin",
                Name = "Admin",
                UserId = Guid.NewGuid(),
                WarehouseId = Guid.NewGuid()
            };

            await ExecuteCommand(async (context) =>
            {
                var userRepository = new UserRepository(context);
                var unitOfWork = new UnitOfWork(context);
                var userConsumer = new UserConsumer(unitOfWork, userRepository, _logger);

                await userConsumer.Consume(eventCreated);

                var user = userRepository.GetById(eventCreated.UserId);
                user.Login.Should().Be(eventCreated.Login);
                user.Name.Should().Be(eventCreated.Name);
                user.WarehouseId.Should().Be(eventCreated.WarehouseId);
            });
        }

        [Fact]
        public async Task should_update_user()
        {
            var user = new User(Guid.NewGuid(), Guid.NewGuid(), "Admin", "admin");

            await ExecuteCommand(async (context) =>
            {
                var userRepository = new UserRepository(context);
                var unitOfWork = new UnitOfWork(context);
                await userRepository.AddAsync(user);
                await unitOfWork.Commit();
            });

            var eventUpdated = new UserUpdatedEvent
            {
                Login = "admin atualizado",
                Name = "Admin Atualizado",
                UserId = user.Id,
                WarehouseId = user.WarehouseId.GetValueOrDefault()
            };

            await ExecuteCommand(async (context) =>
            {
                var userRepository = new UserRepository(context);
                var unitOfWork = new UnitOfWork(context);
                var userConsumer = new UserConsumer(unitOfWork, userRepository, _logger);

                await userConsumer.Consume(eventUpdated);

                var userUpdated = userRepository.GetById(eventUpdated.UserId);
                userUpdated.Login.Should().Be(eventUpdated.Login);
                userUpdated.Name.Should().Be(eventUpdated.Name);
                userUpdated.WarehouseId.Should().Be(eventUpdated.WarehouseId);
            });
        }

        [Fact]
        public async Task should_create_when_user_not_found()
        {
            var eventUpdated = new UserUpdatedEvent
            {
                Login = "admin atualizado",
                Name = "Admin Atualizado",
                UserId = Guid.NewGuid(),
                WarehouseId = Guid.NewGuid()
            };

            await ExecuteCommand(async (context) =>
            {
                var userRepository = new UserRepository(context);
                var unitOfWork = new UnitOfWork(context);
                var userConsumer = new UserConsumer(unitOfWork, userRepository, _logger);

                await userConsumer.Consume(eventUpdated);

                var userUpdated = userRepository.GetById(eventUpdated.UserId);
                userUpdated.Login.Should().Be(eventUpdated.Login);
                userUpdated.Name.Should().Be(eventUpdated.Name);
                userUpdated.WarehouseId.Should().Be(eventUpdated.WarehouseId);
            });
        }
    }
}
