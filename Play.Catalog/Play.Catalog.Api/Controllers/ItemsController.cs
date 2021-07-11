using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Play.Catalog.Application.Contracts.v1.Items;
using Play.Catalog.Application.Interfaces;
using Play.Catalog.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Play.Catalog.Api.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class ItemsController : ControllerBase
    {
        private readonly IRepository<Item> _itemsRepository;
        private readonly IMapper _mapper;

        public ItemsController(IMapper mapper, IRepository<Item> itemsRepository)
        {
            _mapper = mapper;
            _itemsRepository = itemsRepository;
        }

        // GET /api/v1/items
        [HttpGet]
        public async Task<ActionResult<List<ItemResponseDto>>> GetAll()
        {
            IReadOnlyCollection<Item> items = await _itemsRepository.GetAllAsync();
            return Ok(_mapper.Map<List<ItemResponseDto>>(items));
        }

        // GET /api/v1/items/{id}
        [HttpGet("{id:guid}")]
        public async Task<ActionResult<ItemResponseDto>> Get(Guid id)
        {
            var item = await _itemsRepository.GetAsync(id);

            if (item == null)
                return NotFound();

            return Ok(_mapper.Map<ItemResponseDto>(item));
        }

        // POST /api/v1/items
        [HttpPost]
        public async Task<ActionResult<ItemResponseDto>> Create([FromBody] CreateItemDto itemDto)
        {
            var item = _mapper.Map<Item>(itemDto);
            item.CreatedDate = DateTimeOffset.UtcNow;

            bool created = await _itemsRepository.CreateAsync(item);

            if (!created)
                return BadRequest();

            return CreatedAtAction(nameof(Get), new { id = item.Id }, item);
        }

        // PUT /api/v1/items/{id}
        [HttpPut("{id:guid}")]
        public async Task<ActionResult> Update(Guid id, [FromBody] UpdateItemDto itemDto)
        {
            var itemToUpdate = await _itemsRepository.GetAsync(id);

            if (itemToUpdate == null)
                return NotFound();

            _mapper.Map(itemDto, itemToUpdate);

            bool updated = await _itemsRepository.UpdateAsync(itemToUpdate);

            if (!updated)
                return BadRequest();

            return NoContent();
        }

        // DELETE /api/v1/items/{id}
        [HttpDelete("{id:guid}")]
        public async Task<ActionResult> Delete(Guid id)
        {
            bool deleted = await _itemsRepository.DeleteAsync(id);

            if (!deleted)
                return NotFound();

            return NoContent();
        }
    }
}