using AutoMapper;
using Play.Catalog.Domain.Entities;

namespace Play.Catalog.Application.Contracts.v1.Items
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