using AutoMapper;
using Hbsis.Wms.Contracts.BillsOfMaterials;
using Hbsis.Wms.Contracts.Items;
using Wms.ProductionLine.Domain.Dto;

namespace Wms.ProductionLine.Api.AutoMapper
{
    public class EventToDtoProfile : Profile
    {
        public EventToDtoProfile()
        {
            CreateMap<ItemCriado, ItemDto>()
                .ForMember(x => x.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(x => x.Code, opt => opt.MapFrom(src => src.Codigo))
                .ForMember(x => x.Description, opt => opt.MapFrom(src => src.Descricao))
                .ForMember(x => x.ItemType, opt => opt.MapFrom(src => src.TipoItem));

            CreateMap<ItemAtualizado, ItemDto>()
                .ForMember(x => x.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(x => x.Code, opt => opt.MapFrom(src => src.Codigo))
                .ForMember(x => x.Description, opt => opt.MapFrom(src => src.Descricao))
                .ForMember(x => x.ItemType, opt => opt.MapFrom(src => src.TipoItem));

            CreateMap<BillOfMaterialsEventBase, IntegralizadoDto>()
                .ForMember(x => x.Id, opt => opt.MapFrom(src => src.Id));
        }
    }
}