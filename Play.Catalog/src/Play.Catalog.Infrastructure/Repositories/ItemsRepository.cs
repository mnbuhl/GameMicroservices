using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MongoDB.Driver;
using Play.Catalog.Application.Interfaces;
using Play.Catalog.Domain.Entities;

namespace Play.Catalog.Infrastructure.Repositories
{
    public class ItemsRepository : IItemsRepository
    {
        private readonly CatalogContext _context;
        
        public ItemsRepository(CatalogContext context)
        {
            _context = context;
        }

        public async Task<IReadOnlyCollection<Item>> GetAllAsync()
        {
            return await _context.Items.Find(_context.FilterBuilder.Empty).ToListAsync();
        }

        public async Task<Item> GetAsync(Guid id)
        {
            FilterDefinition<Item> filter = _context.FilterBuilder.Eq(entity => entity.Id, id);
            return await _context.Items.Find(filter).FirstOrDefaultAsync();
        }

        public async Task<bool> CreateAsync(Item item)
        {
            if (item == null)
                return false;
            
            await _context.Items.InsertOneAsync(item);
            return true;
        }

        public async Task<bool> UpdateAsync(Item item)
        {
            if (item == null)
                return false;
            
            FilterDefinition<Item> filter = _context.FilterBuilder.Eq(existingEntity => existingEntity.Id, item.Id);

            var result = await _context.Items.ReplaceOneAsync(filter, item);
            
            return result.IsModifiedCountAvailable && result.ModifiedCount > 0;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            FilterDefinition<Item> filter = _context.FilterBuilder.Eq(entity => entity.Id, id);

            var result = await _context.Items.DeleteOneAsync(filter);

            return result.DeletedCount > 0;
        }
    }
}