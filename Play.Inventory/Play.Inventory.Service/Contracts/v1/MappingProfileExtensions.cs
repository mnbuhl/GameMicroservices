using Play.Inventory.Service.Entities;
using System;

namespace Play.Inventory.Service.Contracts.v1
{
    public static class MappingProfileExtensions
    {
        public static InventoryItemDto AsDto(this InventoryItem item)
        {
            return new(item.Id, item.CatalogItemId, item.UserId, item.Quantity, item.AcquiredDate);
        }

        public static InventoryItem AsEntity(this GrantItemsDto itemDto)
        {
            return new()
            {
                Id = Guid.NewGuid(),
                CatalogItemId = itemDto.CatalogItemId,
                UserId = itemDto.UserId,
                Quantity = itemDto.Quantity,
                AcquiredDate = DateTimeOffset.Now
            };
        }
    }
}
