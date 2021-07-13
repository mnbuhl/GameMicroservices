using MassTransit;
using Play.Catalog.Contracts;
using Play.Common;
using Play.Inventory.Service.Entities;
using System.Threading.Tasks;

namespace Play.Inventory.Service.Consumers
{
    public class CatalogItemCreatedConsumer : IConsumer<CatalogItemCreated>
    {
        private readonly IRepository<CatalogItem> _catalogRepository;

        public CatalogItemCreatedConsumer(IRepository<CatalogItem> catalogRepository)
        {
            _catalogRepository = catalogRepository;
        }

        public async Task Consume(ConsumeContext<CatalogItemCreated> context)
        {
            var message = context.Message;

            var item = await _catalogRepository.GetAsync(message.Id);

            if (item != null)
                return;

            item = new CatalogItem
            {
                Id = message.Id,
                Name = message.Name,
                Description = message.Description
            };

            await _catalogRepository.CreateAsync(item);
        }
    }
}