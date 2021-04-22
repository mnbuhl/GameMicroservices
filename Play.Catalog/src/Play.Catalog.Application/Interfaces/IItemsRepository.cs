using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Play.Catalog.Domain.Entities;

namespace Play.Catalog.Application.Interfaces
{
    public interface IItemsRepository
    {
       Task<IReadOnlyCollection<Item>> GetAllAsync();
       Task<Item> GetAsync(Guid id);
       Task<bool> CreateAsync(Item item);
       Task<bool> UpdateAsync(Item item);
       Task<bool> DeleteAsync(Guid id);
    }
}