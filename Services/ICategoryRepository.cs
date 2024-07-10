
using PlateWebAPI.Entities;

namespace PlateWebAPI.Services
{
    public interface ICategoryRepository
    {
        public IEnumerable<Category> AllCategories { get; }
        public Task<Category?> GeCategoryByIdAsync(int? id);

        public Task DeleteAsync(int id);
        public Task<Category?> FindAsync(int? id);
        public Task<bool> CategoryExistsAsync(int id);
        public Task AddAsync(Category category);
        public Task UpdateAsync(Category category);
        public Task<bool> DoesCategoryHaveItemsAsync(int id);

        public Task<bool> SaveChangesAsync();
    }
}
