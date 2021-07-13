using MassTransit;
using Play.Catalog.Contracts;
using Play.Common;
using Play.Inventory.Service.Entities;
using System.Threading.Tasks;

namespace Play.Inventory.Service.Consumers
{
    public class CatalogItemUpdatedConsumer : IConsumer<CatalogItemUpdated>
    {
        private readonly IRepository<CatalogItem> _catalogRepository;

        public CatalogItemUpdatedConsumer(IRepository<CatalogItem> catalogRepository)
        {
            _catalogRepository = catalogRepository;
        }

        public async Task Consume(ConsumeContext<CatalogItemUpdated> context)
        {
            var message = context.Message;

            var item = await _catalogRepository.GetAsync(message.Id);

            if (item == null)
            {
                item = new CatalogItem
                {
                    Id = message.Id,
                    Name = message.Name,
                    Description = message.Description
                };

                await _catalogRepository.CreateAsync(item);
                return;
            }

            item.Name = message.Name;
            item.Description = message.Description;

            await _catalogRepository.UpdateAsync(item);
        }
    }
}