using Microsoft.AspNetCore.Mvc;
using Play.Common;
using Play.Inventory.Service.Contracts.v1;
using Play.Inventory.Service.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Play.Inventory.Service.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class ItemsController : ControllerBase
    {
        private readonly IRepository<InventoryItem> _inventoryRepository;

        public ItemsController(IRepository<InventoryItem> inventoryRepository)
        {
            _inventoryRepository = inventoryRepository;
        }

        [HttpGet("{userId:guid}")]
        public async Task<ActionResult<IEnumerable<InventoryItemDto>>> GetUserInventoryAsync(Guid userId)
        {
            if (userId == Guid.Empty)
                return BadRequest();

            var inventoryItems = (await _inventoryRepository.GetAllAsync(item => item.UserId == userId))
                .Select(item => item.AsDto());

            return Ok(inventoryItems);
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

            return Ok(inventoryItem.AsDto());
        }
    }
}
