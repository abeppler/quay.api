using Hbsis.Wms.Infra.Data.Repository;
using Hbsis.Wms.Infra.Domain.Interfaces.Data;
using Hbsis.Wms.Infra.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Wms.ProductionLine.Api.Application;
using Wms.ProductionLine.Domain.Intefaces;
using Wms.ProductionLine.Domain.Intefaces.Configurations;
using Wms.ProductionLine.Domain.Services.Configurations;
using Wms.ProductionLine.Infra.Data.Context;
using Wms.ProductionLine.Infra.Repository;
using Wms.ProductionLine.Infra.Repository.Configurations;

namespace Wms.ProductionLine.Api
{
    public static class Bootstrapper
    {
        public static IServiceCollection RegisterServices(this IServiceCollection services)
        {
            // Infra
            services.AddScoped<DbContext, ProductionLineContext>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IScopeContext, ScopeContext>();
            services.AddScoped<IProductionLineConfigurationRepository, ProductionLineConfigurationRepository>();
            services.AddScoped<IProductionLineConfigurationHistoryRepository, ProductionLineConfigurationHistoryRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IAddressRepository, AddressRepository>();
            services.AddScoped<IItemRepository, ItemRepository>();
            services.AddScoped<IIntegralizadoRepository, IntegralizadoRepository>();
            services.AddScoped<IIntegralizadoDetailRepository, IntegralizadoDetailRepository>();

            //Application
            services.AddScoped<IConfigurationApplicationService, ConfigurationApplicationService>();
            services.AddScoped<IConfigurationHistoryDomainService, ConfigurationHistoryDomainService>();

            return services;
        }
    }
}
