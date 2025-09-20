using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using HierarchicalDirectory.Domain;

namespace HierarchicalDirectory.Infrastructure
{
    public class CategoryRepository : IRepository<Category>
    {
        private readonly DirectoryDbContext _context;
        public CategoryRepository(DirectoryDbContext context)
        {
            _context = context;
        }
        public async Task<Category> GetByIdAsync(string id)
        {
            return await _context.Categories.Include(c => c.Children).FirstOrDefaultAsync(c => c.Id == id);
        }
        public async Task<IEnumerable<Category>> GetAllAsync()
        {
            return await _context.Categories.Include(c => c.Children).ToListAsync();
        }
        public async Task AddAsync(Category entity)
        {
            _context.Categories.Add(entity);
            await _context.SaveChangesAsync();
        }
        public async Task UpdateAsync(Category entity)
        {
            _context.Categories.Update(entity);
            await _context.SaveChangesAsync();
        }
        public async Task DeleteAsync(string id)
        {
            var entity = await _context.Categories.FindAsync(id);
            if (entity != null)
            {
                _context.Categories.Remove(entity);
                await _context.SaveChangesAsync();
            }
        }
    }
}
