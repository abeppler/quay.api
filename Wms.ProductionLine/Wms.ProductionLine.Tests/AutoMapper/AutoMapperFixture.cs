using AutoMapper;
using Wms.ProductionLine.Api.AutoMapper;

namespace Wms.ProductionLine.Tests.AutoMapper
{
    public sealed class AutoMapperFixture
    {
        private static IMapper instance;

        private AutoMapperFixture() { }

        private static IMapper Initialize()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new EventToDtoProfile());
            });
            return config.CreateMapper();
        }

        public static IMapper GetInstance()
        {
            if (instance == null)
            {
                instance = Initialize();
            }

            return instance;
        }
    }
}
