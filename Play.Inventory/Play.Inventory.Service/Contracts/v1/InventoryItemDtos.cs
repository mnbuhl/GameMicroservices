using System;

namespace Play.Inventory.Service.Contracts.v1
{
    public record InventoryItemDto(Guid Id, Guid CatalogItemId, Guid UserId, int Quantity, DateTimeOffset AcquiredDate);
}