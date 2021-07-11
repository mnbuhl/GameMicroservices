using AutoMapper;
using Play.Catalog.Service.Entities;

namespace Play.Catalog.Service.Contracts.v1.Items
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<CreateItemDto, Item>();
            CreateMap<UpdateItemDto, Item>();
            CreateMap<Item, ItemResponseDto>();
        }
    }
}