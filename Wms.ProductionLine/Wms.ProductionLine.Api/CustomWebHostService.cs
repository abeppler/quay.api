using Hbsis.Wms.Infra.RabbitMq;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.WindowsServices;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Wms.ProductionLine.Api
{
    public class CustomWebHostService : WebHostService
    {
        private ILogger _logger;
        private readonly IMessageSubscribe _messageSubscribe;

        public CustomWebHostService(IWebHost host) : base(host)
        {
            _logger = host.Services.GetRequiredService<ILogger<CustomWebHostService>>();
            _messageSubscribe = host.Services.GetRequiredService<IMessageSubscribe>();
        }

        protected override void OnStopping()
        {
            _logger.LogDebug("OnStopping method called.");

            if (_messageSubscribe != null)
            {
                _messageSubscribe.Dispose();
                _logger.LogDebug("disposed message subscribe.");
            }

            base.OnStopping();
        }
    }
}
