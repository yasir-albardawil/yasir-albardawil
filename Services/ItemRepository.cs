using PlateWebAPI.DBContext;
using PlateWebAPI.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PlateWebAPI.Services
{
    public class ItemRepository : IItemRepository
    {
        private readonly PlateDbContext _context;

        public ItemRepository(PlateDbContext context)
        {
            _context = context;
        }

        public IEnumerable<Item> AllItems
        {
            get
            {
                return _context.Items.Include(c => c.Category);
            }
        }

        public async Task<IEnumerable<Item>> GetItemsAsync()
        {
            return await _context.Items.Include(c => c.Category).ToListAsync();
        }

        public async Task<IEnumerable<Item>> GetItemsOfTheWeekAsync()
        {
            return await _context.Items.Include(c => c.Category).Where(p => p.IsPieOfTheWeek).ToListAsync();
        }

        public async Task<Item?> GetItemByIdAsync(int? id)
        {
            return await _context.Items.Include(p => p.Category).FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<IEnumerable<Item>> GetItemsByCategoryNameAsync(string? name)
        {
            return await _context.Items.Include(p => p.Category).Where(m => m.Category.CategoryName == name).ToArrayAsync();
        }

        public async Task CreateAsync(Item item)
        {
            await _context.AddAsync(item);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Item pie)
        {
            _context.Attach(pie.Category);
            _context.Update(pie);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var item = await _context.Items.FindAsync(id);
            if (item != null)
            {
                _context.Remove(item);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<Item?> FindAsync(int? id)
        {
            return await _context.Items.FindAsync(id);
        }

        public async Task<bool> ExistsAsync(int? id)
        {
            return await _context.Items.AnyAsync(p => p.Id == id);
        }

        public async Task<bool> SaveChangesAsync()
        {
            return (await _context.SaveChangesAsync() >= 0);
        }
    }
}
