using AutoMapper;

namespace Wms.ProductionLine.Api.AutoMapper
{
    public class AutoMapperConfiguration
    {
        public static MapperConfiguration RegisterMappings()
        {
            return new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new EventToDtoProfile());
            });
        }
    }
}
