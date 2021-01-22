using Hbsis.Wms.Contracts.Addresses;
using Hbsis.Wms.Infra.Domain.Interfaces.Data;
using Hbsis.Wms.Infra.RabbitMq;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using Wms.ProductionLine.Domain.Dto;
using Wms.ProductionLine.Domain.Entities;
using Wms.ProductionLine.Domain.Intefaces;

namespace Wms.ProductionLine.Api.Consumers
{
    public class AddressConsumer : IBusConsumer<EnderecoAtualizado>, IBusConsumer<EnderecoCriado>
    {
        private readonly IUnitOfWork _uow;
        private readonly IAddressRepository _addressRepository;
        private readonly ILogger<AddressConsumer> _logger;

        public AddressConsumer(IUnitOfWork uow, IAddressRepository addressRepository, ILogger<AddressConsumer> logger)
        {
            _uow = uow;
            _addressRepository = addressRepository;
            _logger = logger;
        }
        public async Task Consume(EnderecoCriado message)
        {
            _logger.LogDebug($"Consuming 'EnderecoCriado'=> Code:{message.Codigo}");

            var addressDto = new AddressDto
            {
                Id = message.Id,
                WarehouseId = message.IdArmazem,
                Code = message.Codigo,
                Description = message.Descricao,
                Enabled = message.Enabled,
                Sequence = message.Sequence,
            };

            var address = new Address(addressDto);
            await _addressRepository.AddAsync(address);
            await _uow.Commit();

            _logger.LogDebug($"Consumed 'EnderecoCriado'=> Code:{message.Codigo}");
        }


        public async Task Consume(EnderecoAtualizado message)
        {
            _logger.LogDebug($"Consuming 'EnderecoAtualizado'=> Code:{message.Codigo}");

            var addressDto = new AddressDto
            {
                Id = message.Id,
                WarehouseId = message.IdArmazem,
                Code = message.Codigo,
                Description = message.Descricao,
                Enabled = message.Enabled,
                Sequence = message.Sequence,
            };

            var address = await _addressRepository.GetByIdAsync(addressDto.Id);
            if (address == null)
            {
                address = new Address(addressDto);
                await _addressRepository.AddAsync(address);
                await _uow.Commit();
                return;
            }

            address.UpdateProperties(addressDto);
            await _addressRepository.UpdateAsync(address);
            await _uow.Commit();

            _logger.LogDebug($"Consumed 'EnderecoAtualizado'=> Code:{message.Codigo}");
        }
    }
}
