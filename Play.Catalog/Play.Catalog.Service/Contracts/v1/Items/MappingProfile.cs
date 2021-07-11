using AutoMapper;
using Play.Catalog.Api.Entities;

namespace Play.Catalog.Api.Contracts.v1.Items
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