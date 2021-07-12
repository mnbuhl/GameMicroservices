using System;

namespace Play.Inventory.Service.Contracts.v1
{
    public record CatalogItemDto(Guid Id, string Name, string Description);
}
