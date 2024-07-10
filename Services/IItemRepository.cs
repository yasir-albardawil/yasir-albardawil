using PlateWebAPI.Entities;
using PlateWebAPI.Services;

namespace PlateWebAPI.Services
{
    public interface IItemRepository
    {
        Task<IEnumerable<Item>> GetItemsAsync();
        Task<IEnumerable<Item>> GetItemsOfTheWeekAsync();
         Task<Item?> GetItemByIdAsync(int? id);

        Task<IEnumerable<Item>> GetItemsByCategoryNameAsync(string? name);
        Task CreateAsync(Item iem);
         Task UpdateAsync(Item item);
        Task DeleteAsync(int id);
        Task<Item?> FindAsync(int? id);
        Task<bool> ExistsAsync(int? id);
        Task<bool> SaveChangesAsync();
    }

}
