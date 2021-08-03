using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Play.Common;
using Play.Inventory.Service.Contracts.v1;
using Play.Inventory.Service.Entities;

namespace Play.Inventory.Service.Controllers
{
    [Authorize]
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class ItemsController : ControllerBase
    {
        private readonly IRepository<InventoryItem> _inventoryRepository;
        private readonly IRepository<CatalogItem> _catalogRepository;

        public ItemsController(IRepository<InventoryItem> inventoryRepository,
            IRepository<CatalogItem> catalogRepository)
        {
            _inventoryRepository = inventoryRepository;
            _catalogRepository = catalogRepository;
        }

        [HttpGet("{userId:guid}")]
        public async Task<ActionResult<IEnumerable<InventoryItemDto>>> GetUserInventoryAsync(Guid userId)
        {
            if (userId == Guid.Empty)
                return BadRequest();

            var inventoryItemEntities = await _inventoryRepository.GetAllAsync(item => item.UserId == userId);
            var catalogItemIds = inventoryItemEntities.Select(item => item.CatalogItemId);
            var catalogItemEntities = await _catalogRepository.GetAllAsync(item => catalogItemIds.Contains(item.Id));

            var inventoryItemDtos = inventoryItemEntities.Select(item =>
            {
                var catalogItemDto =
                    catalogItemEntities.FirstOrDefault(catalogItem => catalogItem.Id == item.CatalogItemId);
                return item.AsDto(catalogItemDto?.Name ?? "Not found", catalogItemDto?.Description ?? "Not found");
            });

            return Ok(inventoryItemDtos);
        }

        [HttpPost]
        public async Task<ActionResult<InventoryItemDto>> GrantItemsAsync(GiveOrTakeItemsDto giveOrTakeItemsDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var inventoryItem = await _inventoryRepository.GetAsync(item =>
                item.UserId == giveOrTakeItemsDto.UserId && item.CatalogItemId == giveOrTakeItemsDto.CatalogItemId);

            if (inventoryItem == null)
            {
                inventoryItem = giveOrTakeItemsDto.AsEntity();

                await _inventoryRepository.CreateAsync(inventoryItem);
            }
            else
            {
                if (giveOrTakeItemsDto.Quantity < 0 && (inventoryItem.Quantity + giveOrTakeItemsDto.Quantity) < 0)
                    return BadRequest("Player can't have negative number of items in inventory.");

                inventoryItem.Quantity += giveOrTakeItemsDto.Quantity;
                await _inventoryRepository.UpdateAsync(inventoryItem);
            }

            var catalogItem = await _catalogRepository.GetAsync(giveOrTakeItemsDto.CatalogItemId);

            return Ok(inventoryItem.AsDto(catalogItem.Name, catalogItem.Description));
        }
    }
}