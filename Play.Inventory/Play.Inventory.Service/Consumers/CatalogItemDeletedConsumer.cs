using MassTransit;
using Play.Catalog.Contracts;
using Play.Common;
using Play.Inventory.Service.Entities;
using System.Threading.Tasks;

namespace Play.Inventory.Service.Consumers
{
    public class CatalogItemDeletedConsumer : IConsumer<CatalogItemDeleted>
    {
        private readonly IRepository<CatalogItem> _catalogRepository;

        public CatalogItemDeletedConsumer(IRepository<CatalogItem> catalogRepository)
        {
            _catalogRepository = catalogRepository;
        }

        public async Task Consume(ConsumeContext<CatalogItemDeleted> context)
        {
            var message = context.Message;

            var item = await _catalogRepository.GetAsync(message.Id);

            if (item == null)
            {
                return;
            }

            await _catalogRepository.DeleteAsync(message.Id);
        }
    }
}