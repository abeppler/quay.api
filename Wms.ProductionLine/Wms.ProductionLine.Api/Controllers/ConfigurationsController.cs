using Hbsis.Wms.Infra.Domain.Interfaces.EventHandling;
using Hbsis.Wms.Infra.Identity;
using Hbsis.Wms.Infra.WebApi.Standard;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Wms.ProductionLine.Api.Application;
using Wms.ProductionLine.Domain.Dto;
using Wms.ProductionLine.Domain.Dto.Filter;
using Wms.ProductionLine.Domain.Intefaces.Configurations;

namespace Wms.ProductionLine.Api.Controllers
{
    [Route("[controller]")]
    public class ConfigurationsController : ApiController
    {
        private readonly IScopeContext _scopeContext;
        private readonly IConfigurationApplicationService _configurationAppService;
        private readonly IConfigurationHistoryDomainService _configurationHistoryDomainService;

        public ConfigurationsController(IDomainNotificationManager notifications
            ,IScopeContext scopeContext
            ,IConfigurationApplicationService configurationAppService
            ,IConfigurationHistoryDomainService configurationHistoryDomainService) : base(notifications)
        {
            _scopeContext = scopeContext;
            _configurationAppService = configurationAppService;
            _configurationHistoryDomainService = configurationHistoryDomainService;
        }

        [HttpGet]
        public IActionResult Get()
        {
            var warehouseId = _scopeContext.GetWarehouseId().Value;
            var configuration = _configurationAppService.GetConfiguration(warehouseId);

            return Response(configuration);
        }

        [HttpPost]
        public async Task<IActionResult> CreateConfiguration([FromBody] ProductionLineConfigurationDto configurationDto)
        {
            configurationDto.WarehouseId = _scopeContext.GetWarehouseId().Value;
            var userId = _scopeContext.GetUserId().GetValueOrDefault();
            var userLogin = _scopeContext.GetUserLogin();

            await _configurationAppService.AddConfiguration(configurationDto, userId, userLogin);

            return Response();
        }

        [HttpPut]
        public async Task<IActionResult> UpdateConfiguration([FromBody] ProductionLineConfigurationDto configurationDto)
        {
            configurationDto.WarehouseId = _scopeContext.GetWarehouseId().Value;
            var userId = _scopeContext.GetUserId().GetValueOrDefault();
            var userLogin = _scopeContext.GetUserLogin();

            await _configurationAppService.UpdateConfiguration(configurationDto, userId, userLogin);

            return Response();
        }

        [HttpGet("history")]
        public async Task<IActionResult> GetHistory([FromQuery] ConfigurationHistoryFilter filter)
        {
            var history = await _configurationHistoryDomainService.GetHistory(filter);

            return Response(history);
        }
    }
}
