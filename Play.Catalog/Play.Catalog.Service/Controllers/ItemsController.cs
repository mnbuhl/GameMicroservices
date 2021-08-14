using AutoMapper;
using MassTransit;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Play.Catalog.Contracts;
using Play.Catalog.Service.Contracts.v1.Items;
using Play.Catalog.Service.Entities;
using Play.Common;

namespace Play.Catalog.Service.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class ItemsController : ControllerBase
    {
        private const string AdminRole = "Admin";

        private readonly IRepository<Item> _itemsRepository;
        private readonly IMapper _mapper;
        private readonly IPublishEndpoint _publishEndpoint;

        public ItemsController(IMapper mapper, IRepository<Item> itemsRepository, IPublishEndpoint publishEndpoint)
        {
            _mapper = mapper;
            _itemsRepository = itemsRepository;
            _publishEndpoint = publishEndpoint;
        }

        // GET /api/v1/items
        [HttpGet]
        [Authorize(Policies.Read)]
        public async Task<ActionResult<List<ItemResponseDto>>> GetAll()
        {
            IReadOnlyCollection<Item> items = await _itemsRepository.GetAllAsync();
            return Ok(_mapper.Map<List<ItemResponseDto>>(items));
        }

        // GET /api/v1/items/{id}
        [HttpGet("{id:guid}")]
        [Authorize(Policies.Read)]
        public async Task<ActionResult<ItemResponseDto>> Get(Guid id)
        {
            var item = await _itemsRepository.GetAsync(id);

            if (item == null)
                return NotFound();

            return Ok(_mapper.Map<ItemResponseDto>(item));
        }

        // POST /api/v1/items
        [HttpPost]
        [Authorize(Policies.Write)]
        public async Task<ActionResult<ItemResponseDto>> Create([FromBody] CreateItemDto itemDto)
        {
            var item = _mapper.Map<Item>(itemDto);
            item.CreatedDate = DateTimeOffset.UtcNow;

            bool created = await _itemsRepository.CreateAsync(item);

            if (!created)
                return BadRequest();

            await _publishEndpoint.Publish(new CatalogItemCreated(item.Id, item.Name, item.Description));

            return CreatedAtAction(nameof(Get), new { id = item.Id }, item);
        }

        // PUT /api/v1/items/{id}
        [HttpPut("{id:guid}")]
        [Authorize(Policies.Write)]
        public async Task<ActionResult> Update(Guid id, [FromBody] UpdateItemDto itemDto)
        {
            var itemToUpdate = await _itemsRepository.GetAsync(id);

            if (itemToUpdate == null)
                return NotFound();

            _mapper.Map(itemDto, itemToUpdate);

            bool updated = await _itemsRepository.UpdateAsync(itemToUpdate);

            if (!updated)
                return BadRequest();

            await _publishEndpoint.Publish(new CatalogItemUpdated(itemToUpdate.Id, itemToUpdate.Name,
                itemToUpdate.Description));

            return NoContent();
        }

        // DELETE /api/v1/items/{id}
        [HttpDelete("{id:guid}")]
        [Authorize(Policies.Write)]
        public async Task<ActionResult> Delete(Guid id)
        {
            bool deleted = await _itemsRepository.DeleteAsync(id);

            if (!deleted)
                return NotFound();

            await _publishEndpoint.Publish(new CatalogItemDeleted(id));

            return NoContent();
        }
    }
}