using System;
using System.ComponentModel.DataAnnotations;

namespace Play.Inventory.Service.Contracts.v1
{
    public record GiveOrTakeItemsDto([Required] Guid UserId, [Required] Guid CatalogItemId, [Required] int Quantity);
}
