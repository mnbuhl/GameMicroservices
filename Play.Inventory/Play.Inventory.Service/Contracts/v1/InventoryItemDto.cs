using System;

namespace Play.Inventory.Service.Contracts.v1
{
    public record InventoryItemDto(
        Guid Id,
        Guid CatalogItemId,
        string Name,
        string Description,
        int Quantity,
        DateTimeOffset AcquiredDate);
}