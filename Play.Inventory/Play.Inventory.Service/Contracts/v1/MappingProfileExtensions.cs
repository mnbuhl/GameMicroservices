using Play.Inventory.Service.Entities;
using System;

namespace Play.Inventory.Service.Contracts.v1
{
    public static class MappingProfileExtensions
    {
        public static InventoryItemDto AsDto(this InventoryItem item, string name, string description)
        {
            return new(item.Id, item.CatalogItemId, name, description, item.Quantity, item.AcquiredDate);
        }

        public static InventoryItem AsEntity(this GiveOrTakeItemsDto giveOrTakeItemsDto)
        {
            return new()
            {
                Id = Guid.NewGuid(),
                CatalogItemId = giveOrTakeItemsDto.CatalogItemId,
                UserId = giveOrTakeItemsDto.UserId,
                Quantity = giveOrTakeItemsDto.Quantity,
                AcquiredDate = DateTimeOffset.Now
            };
        }
    }
}
