using Hbsis.Wms.Contracts.Addresses;
using Hbsis.Wms.Contracts.BillsOfMaterials;
using Hbsis.Wms.Contracts.Items;
using Hbsis.Wms.Contracts.Users;
using Hbsis.Wms.Infra.RabbitMq;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Wms.ProductionLine.Api
{
    public static class RabbitMqListener
    {
        public static void SubscribeQueues(IServiceProvider serviceProvider)
        {
            var messageSubscribe = serviceProvider.GetRequiredService<IMessageSubscribe>();
            messageSubscribe.HealthCheck(serviceProvider);

            messageSubscribe.Subscribe<UserCreatedEvent>(serviceProvider);
            messageSubscribe.Subscribe<UserUpdatedEvent>(serviceProvider);

            messageSubscribe.Subscribe<EnderecoCriado>(serviceProvider);
            messageSubscribe.Subscribe<EnderecoAtualizado>(serviceProvider);

            messageSubscribe.Subscribe<CreatedBillOfMaterials>(serviceProvider);
            messageSubscribe.Subscribe<AlteredBillOfMaterials>(serviceProvider);
            messageSubscribe.Subscribe<DeletedBillOfMaterials>(serviceProvider);

            messageSubscribe.Subscribe<ItemCriado>(serviceProvider);
            messageSubscribe.Subscribe<ItemAtualizado>(serviceProvider);
        }
    }
}
