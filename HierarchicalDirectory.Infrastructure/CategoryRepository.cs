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
            // Проверка уникальности имени среди дочерних элементов одного родителя
            if (!string.IsNullOrEmpty(entity.ParentId))
            {
                var exists = await _context.Categories.AnyAsync(c => c.ParentId == entity.ParentId && c.Name == entity.Name);
                if (exists)
                    throw new System.Exception($"Category name '{entity.Name}' must be unique within parent '{entity.ParentId}'.");
            }
            _context.Categories.Add(entity);
            await _context.SaveChangesAsync();
        }
        public async Task UpdateAsync(Category entity)
        {
            // Проверка уникальности имени среди дочерних элементов одного родителя
            if (!string.IsNullOrEmpty(entity.ParentId))
            {
                var exists = await _context.Categories.AnyAsync(c => c.ParentId == entity.ParentId && c.Name == entity.Name && c.Id != entity.Id);
                if (exists)
                    throw new System.Exception($"Category name '{entity.Name}' must be unique within parent '{entity.ParentId}'.");
            }
            _context.Categories.Update(entity);
            await _context.SaveChangesAsync();
        }
        public async Task DeleteAsync(string id)
        {
            var entity = await _context.Categories.Include(c => c.Children).FirstOrDefaultAsync(c => c.Id == id);
            if (entity != null)
            {
                // Рекурсивное удаление всех дочерних элементов
                await DeleteChildrenRecursive(entity);
                _context.Categories.Remove(entity);
                await _context.SaveChangesAsync();
            }

        private async Task DeleteChildrenRecursive(Category parent)
        {
            foreach (var child in parent.Children)
            {
                var childEntity = await _context.Categories.Include(c => c.Children).FirstOrDefaultAsync(c => c.Id == child.Id);
                if (childEntity != null)
                {
                    await DeleteChildrenRecursive(childEntity);
                    _context.Categories.Remove(childEntity);
                }
            }
        }
        }
    }
}
