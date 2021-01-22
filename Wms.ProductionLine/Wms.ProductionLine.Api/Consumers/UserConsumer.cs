using Hbsis.Wms.Contracts.Users;
using Hbsis.Wms.Infra.Domain.Interfaces.Data;
using Hbsis.Wms.Infra.RabbitMq;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using Wms.ProductionLine.Domain.Dto;
using Wms.ProductionLine.Domain.Entities;
using Wms.ProductionLine.Domain.Intefaces;

namespace Wms.ProductionLine.Api.Consumers
{
    public class UserConsumer : IBusConsumer<UserCreatedEvent>, IBusConsumer<UserUpdatedEvent>
    {
        private readonly IUnitOfWork _uow;
        private readonly IUserRepository _userRepository;
        private readonly ILogger<UserConsumer> _logger;

        public UserConsumer(IUnitOfWork uow, IUserRepository userRepository, ILogger<UserConsumer> logger)
        {
            _uow = uow;
            _userRepository = userRepository;
            _logger = logger;
        }

        public async Task Consume(UserCreatedEvent message)
        {
            _logger.LogDebug($"Consuming 'UserCreatedEvent'=> UserId:{message.UserId}");
            var user = new User(message.UserId, message.WarehouseId, message.Name, message.Login);
            await _userRepository.AddAsync(user);
            await _uow.Commit();

            _logger.LogDebug($"Consumed 'UserCreatedEvent'=> UserId:{message.UserId}");
        }

        public async Task Consume(UserUpdatedEvent message)
        {
            _logger.LogDebug($"Consuming 'UserUpdatedEvent'=> UserId:{message.UserId}");
            var user = await _userRepository.GetByIdAsync(message.UserId);

            if (user == null)
            {
                user = new User(message.UserId, message.WarehouseId, message.Name, message.Login);
                await _userRepository.AddAsync(user);
                await _uow.Commit();
                return;
            }

            var userDto = new UserDto
            {
                Login = message.Login,
                Name = message.Name,
                WarehouseId = message.WarehouseId,
            };

            user.UpdateProperties(userDto);
            await _userRepository.UpdateAsync(user);
            await _uow.Commit();

            _logger.LogDebug($"Consumed 'UserUpdatedEvent'=> UserId:{message.UserId}");
        }
    }
}
