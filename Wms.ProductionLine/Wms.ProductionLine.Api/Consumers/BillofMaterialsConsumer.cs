using AutoMapper;
using Hbsis.Wms.Contracts.BillsOfMaterials;
using Hbsis.Wms.Infra.Domain.Interfaces.Data;
using Hbsis.Wms.Infra.RabbitMq;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Threading.Tasks;
using Wms.ProductionLine.Domain.Dto;
using Wms.ProductionLine.Domain.Entities;
using Wms.ProductionLine.Domain.Intefaces;

namespace Wms.ProductionLine.Api.Consumers
{
    public class BillOfMaterialsConsumer : IBusConsumer<CreatedBillOfMaterials>,
        IBusConsumer<AlteredBillOfMaterials>,
        IBusConsumer<DeletedBillOfMaterials>
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IIntegralizadoRepository _integralizadoRepository;
        private readonly IIntegralizadoDetailRepository _integralizadoDetailRepository;
        private readonly ILogger<BillOfMaterialsConsumer> _logger;
        
        public BillOfMaterialsConsumer(IUnitOfWork unitOfWork, IMapper mapper, IIntegralizadoRepository integralizadoRepository, 
            IIntegralizadoDetailRepository integralizadoDetailRepository, ILogger<BillOfMaterialsConsumer> logger)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _integralizadoRepository = integralizadoRepository;
            _integralizadoDetailRepository = integralizadoDetailRepository;
            _logger = logger;
        }

        public async Task Consume(CreatedBillOfMaterials message)
        {
            _logger.LogDebug($"Consuming CreatedBillOfMaterials => Id: {message.Id}");

            await AddIntegralizado(message);

            _logger.LogDebug($"Consumed CreatedBillOfMaterials => Id: {message.Id}");
        }

        public async Task Consume(AlteredBillOfMaterials message)
        {
            _logger.LogDebug($"Consuming AlteredBillOfMaterials => Id: {message.Id}");

            var integralizado = await _integralizadoRepository.GetWithDetails(message.Id);

            if (integralizado == null)
            {
                await AddIntegralizado(message);
                return;
            }

            var integralizadoDto = _mapper.Map<IntegralizadoDto>(message);
            await DeleteDetails(integralizado);

            integralizado.UpdateProperties(integralizadoDto);
            await _unitOfWork.Commit();

            _logger.LogDebug($"Consumed AlteredBillOfMaterials => Id: {message.Id}");
        }

        public async Task Consume(DeletedBillOfMaterials message)
        {
            _logger.LogDebug($"Consuming DeletedBillOfMaterials => Id: {message.Id}");

            var integralizado   = await _integralizadoRepository.GetWithDetails(message.Id);

            if (integralizado == null)
                return;

            await DeleteDetails(integralizado);
            await _integralizadoRepository.Delete(integralizado);
            
            await _unitOfWork.Commit();           

            _logger.LogDebug($"Consumed DeletedBillOfMaterials => Id: {message.Id}");
        }

        private async Task AddIntegralizado(BillOfMaterialsEventBase message)
        {
            var integralizadoDetails = message.Details.Select(x => new IntegralizadoDetail(x.Id, x.ItemId, x.Quantity)).ToList();
            var integralizado = new Integralizado(message.Id, message.Item, message.Enabled, message.WarehouseId, integralizadoDetails);

            await _integralizadoRepository.AddAsync(integralizado);
            await _unitOfWork.Commit();
        }

        private async Task DeleteDetails(Integralizado integralizado)
        {
            var detailsToRemove = integralizado.Details.Select(i => i.Id).ToArray();
            await _integralizadoDetailRepository.Delete(detailsToRemove);
        }
    }
}
