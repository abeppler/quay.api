using System.Threading.Tasks;
using AutoMapper;
using Hbsis.Wms.Contracts.Items;
using Hbsis.Wms.Infra.Domain.Interfaces.Data;
using Hbsis.Wms.Infra.Domain.Interfaces.Data;
using Hbsis.Wms.Infra.RabbitMq;
using Microsoft.Extensions.Logging;
using Wms.ProductionLine.Domain.Dto;
using Wms.ProductionLine.Domain.Entities;
using Wms.ProductionLine.Domain.Intefaces;

namespace Wms.ProductionLine.Api.Consumers
{
    public class ItemConsumer : IBusConsumer<ItemCriado>, IBusConsumer<ItemAtualizado>
    {
        private IUnitOfWork _unitOfWork;
        private IItemRepository _itemRepository;
        private ILogger<ItemConsumer> _logger;
        private IMapper _mapper;

        public ItemConsumer(IUnitOfWork unitOfWork, IMapper mapper, ILogger<ItemConsumer> logger, IItemRepository itemRepository)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
            _itemRepository = itemRepository;
        }

        public async Task Consume(ItemCriado message)
        {
            _logger.LogDebug($"Consuming 'ItemCriado' => ItemId: {message.Id}");

            var item = new Item(message.Id, message.Descricao, message.Codigo, message.TipoItem);

            await _itemRepository.AddAsync(item);
            await _unitOfWork.Commit();

            _logger.LogDebug($"Consumed 'ItemCriado' => ItemId: {message.Id}");
        }

        public async Task Consume(ItemAtualizado message)
        {
            _logger.LogDebug($"Consuming 'ItemAtualizado' => ItemId: {message.Id}");
            var item = await _itemRepository.GetByIdAsync(message.Id);

            if (item == null)
            {
                item = new Item(message.Id, message.Descricao, message.Codigo, message.TipoItem);

                await _itemRepository.AddAsync(item);
                await _unitOfWork.Commit();
                return;
            }

            var itemDto = _mapper.Map<ItemDto>(message);            

            item.UpdateProperties(itemDto);
            await _unitOfWork.Commit();

            _logger.LogDebug($"Consumed 'ItemAtualizado' => ItemId: {message.Id}");
        }
    }
}
