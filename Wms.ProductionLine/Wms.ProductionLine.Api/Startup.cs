using AutoMapper;
using Hbsis.Wms.Contracts.Addresses;
using Hbsis.Wms.Contracts.BillsOfMaterials;
using Hbsis.Wms.Contracts.Items;
using Hbsis.Wms.Contracts.Users;
using Hbsis.Wms.Infra.RabbitMq;
using Hbsis.Wms.Infra.WebApi.Standard;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerUI;
using Wms.ProductionLine.Api.Consumers;
using Wms.ProductionLine.Infra.Data.Context;

namespace Wms.ProductionLine.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddOptions();
            services.AddDbContext<ProductionLineContext>(options => options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            services.WmsConfigureServices(Configuration, "Wms.ProductionLine", typeof(Startup), new Info
            {
                Version = "1.0",
                Title = "Production Line",
                Description = "Production line service"
            });

            services.AddAutoMapper();
            AutoMapperConfiguration.RegisterMappings();

            services.RegisterServices();

            services.AddScoped<IBusConsumer<UserCreatedEvent>, UserConsumer>();
            services.AddScoped<IBusConsumer<UserUpdatedEvent>, UserConsumer>();

            services.AddScoped<IBusConsumer<EnderecoCriado>, AddressConsumer>();
            services.AddScoped<IBusConsumer<EnderecoAtualizado>, AddressConsumer>();

            services.AddScoped<IBusConsumer<ItemCriado>, ItemConsumer>();
            services.AddScoped<IBusConsumer<ItemAtualizado>, ItemConsumer>();

            services.AddScoped<IBusConsumer<CreatedBillOfMaterials>, BillOfMaterialsConsumer>();
            services.AddScoped<IBusConsumer<AlteredBillOfMaterials>, BillOfMaterialsConsumer>();
            services.AddScoped<IBusConsumer<DeletedBillOfMaterials>, BillOfMaterialsConsumer>();

            services.AddSingleton<IMessageSubscribe>(new MessageSubscribe(services.BuildServiceProvider()));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            using (var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
            {
                var context = serviceScope.ServiceProvider.GetRequiredService<ProductionLineContext>();
                context.Database.Migrate();
            }

            app.UseCors(c =>
            {
                c.AllowAnyHeader();
                c.AllowAnyMethod();
                c.AllowAnyOrigin();
            });

            RabbitMqListener.SubscribeQueues(app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope().ServiceProvider);
            var swaggerOptions = new SwaggerUIOptions();
            swaggerOptions.SwaggerEndpoint("/api/production-line/swagger/v1/swagger.json", "Production Line MicroService V1");
            app.WmsConfigureApplication(env, swaggerOptions);
        }
    }
}
