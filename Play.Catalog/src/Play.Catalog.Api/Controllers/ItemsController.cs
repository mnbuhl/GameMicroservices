using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Play.Catalog.Application.Contracts.v1.Items;

namespace Play.Catalog.Api.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class ItemsController : ControllerBase
    {
        private static readonly List<ItemResponseDto> Items = new List<ItemResponseDto>
        {
            new ItemResponseDto(Guid.NewGuid(), "Potion", "Restores small amount of HP", 50, DateTimeOffset.UtcNow),
            new ItemResponseDto(Guid.NewGuid(), "Super Potion", "Restores medium amount of HP", 100, DateTimeOffset.UtcNow),
            new ItemResponseDto(Guid.NewGuid(), "Antidote", "Cures poisoned player", 75, DateTimeOffset.UtcNow)
        };

        private readonly IMapper _mapper;
        
        public ItemsController(IMapper mapper)
        {
            _mapper = mapper;
        }
        
        // GET /api/v1/items
        [HttpGet]
        public ActionResult<List<ItemResponseDto>> GetAll()
        {
            return Ok(Items);
        }
        
        // GET /api/v1/items/{id}
        [HttpGet("{id:guid}")]
        public ActionResult<ItemResponseDto> Get(Guid id)
        {
            var item = Items.SingleOrDefault(i => i.Id == id);

            if (item == null)
                return NotFound();

            return Ok(item);
        }

        // POST /api/v1/items
        [HttpPost]
        public ActionResult<ItemResponseDto> Create([FromBody] CreateItemDto itemDto)
        {
            var item = new ItemResponseDto(Guid.NewGuid(), itemDto.Name, itemDto.Description, itemDto.Price,
                DateTimeOffset.UtcNow);
            
            Items.Add(item);

            return CreatedAtAction(nameof(Get), new { id = item.Id }, item);
        }

        // PUT /api/v1/items/{id}
        [HttpPut("{id:guid}")]
        public ActionResult Update(Guid id, [FromBody] UpdateItemDto itemDto)
        {
            var itemToUpdate = Items.SingleOrDefault(x => x.Id == id);

            if (itemToUpdate == null)
            {
                return NotFound();
            }

            var updatedItem = itemToUpdate with
            {
                Name = itemDto.Name,
                Description = itemDto.Description,
                Price = itemDto.Price
            };

            int index = Items.FindIndex(x => x.Id == id);

            Items[index] = updatedItem;

            return NoContent();
        }

        // DELETE /api/v1/items/{id}
        [HttpDelete("{id:guid}")]
        public ActionResult Delete(Guid id)
        {
            var itemToRemove = Items.SingleOrDefault(x => x.Id == id);

            if (itemToRemove == null)
                return NotFound();

            Items.Remove(itemToRemove);

            return NoContent();
        }
    }
}