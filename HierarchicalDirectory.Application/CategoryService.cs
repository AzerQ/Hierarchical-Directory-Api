using System.Collections.Generic;
using System.Threading.Tasks;
using HierarchicalDirectory.Domain;

namespace HierarchicalDirectory.Application
{
    public class CategoryService : ICategoryService
    {
        private readonly IRepository<Category> _repository;
        public CategoryService(IRepository<Category> repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<CategoryDto>> GetAllAsync(int? depth = null, string? search = null)
        {
            var categories = await _repository.GetAllAsync();
            // Фильтрация по search
            if (!string.IsNullOrEmpty(search))
                categories = categories.Where(c => c.Name.Contains(search));
            // Получение только корневых узлов
            var roots = categories.Where(c => c.ParentId == null).ToList();
            return roots.Select(c => MapToDto(c, depth ?? int.MaxValue)).ToList();
        }

        public async Task<CategoryDto> GetByIdAsync(string id, bool includeChildren = false)
        {
            var category = await _repository.GetByIdAsync(id);
            if (category == null) return null;
            return MapToDto(category, includeChildren ? int.MaxValue : 0);
        }

        public async Task<IEnumerable<CategoryDto>> GetBatchAsync(IEnumerable<string> ids, bool includeChildren = false)
        {
            var result = new List<CategoryDto>();
            foreach (var id in ids)
            {
                var category = await _repository.GetByIdAsync(id);
                if (category != null)
                    result.Add(MapToDto(category, includeChildren ? int.MaxValue : 0));
            }
            return result;
        }

        public async Task<CategoryDto> CreateAsync(CategoryDto dto)
        {
            var entity = new Category
            {
                Id = Guid.NewGuid().ToString(),
                Name = dto.Name,
                ParentId = dto.ParentId,
                Data = dto.Data?.ToString(),
                Schema = dto.Schema?.ToString(),
                IsLatest = true,
                LoadDate = DateTime.UtcNow
            };
            await _repository.AddAsync(entity);
            return MapToDto(entity, 0);
        }

        public async Task<CategoryDto> UpdateAsync(string id, CategoryDto dto)
        {
            var entity = await _repository.GetByIdAsync(id);
            if (entity == null) return null;
            entity.Name = dto.Name ?? entity.Name;
            entity.Data = dto.Data?.ToString() ?? entity.Data;
            entity.Schema = dto.Schema?.ToString() ?? entity.Schema;
            await _repository.UpdateAsync(entity);
            return MapToDto(entity, 0);
        }

        public async Task DeleteAsync(string id)
        {
            await _repository.DeleteAsync(id);
        }

        public async Task<CategoryDto> CreateLeafVersionAsync(string parentId, CategoryDto dto)
        {
            // Получить все листовые элементы с этим parentId и productCode
            var all = await _repository.GetAllAsync();
            var leaves = all.Where(c => c.ParentId == parentId && c.Children.Count == 0 && c.Data != null && c.Data.Contains(dto.Data?.ToString())).ToList();
            // Сбросить isLatest у предыдущих версий
            foreach (var leaf in leaves)
            {
                leaf.IsLatest = false;
                await _repository.UpdateAsync(leaf);
            }
            var entity = new Category
            {
                Id = Guid.NewGuid().ToString(),
                Name = dto.Name,
                ParentId = parentId,
                Data = dto.Data?.ToString(),
                IsLatest = true,
                LoadDate = DateTime.UtcNow
            };
            await _repository.AddAsync(entity);
            return MapToDto(entity, 0);
        }

        public async Task<(bool valid, List<string> errors)> ValidateAsync(string id, object data)
        {
            // Получить узел
            var category = await _repository.GetByIdAsync(id);
            if (category == null)
                return (false, new List<string> { "Category not found" });

            // Рекурсивно найти схему
            string schema = await FindSchemaRecursive(category);
            if (string.IsNullOrEmpty(schema))
                return (true, new List<string>()); // Нет схемы — валидация не требуется

            // Преобразовать data в строку JSON
            string dataJson = data is string str ? str : Newtonsoft.Json.JsonConvert.SerializeObject(data);
            var result = JsonSchemaValidator.Validate(schema, dataJson);
            return result;

        private async Task<string> FindSchemaRecursive(Category category)
        {
            if (!string.IsNullOrEmpty(category.Schema))
                return category.Schema;
            if (category.ParentId == null)
                return null;
            var parent = await _repository.GetByIdAsync(category.ParentId);
            if (parent == null)
                return null;
            return await FindSchemaRecursive(parent);
        }
        }

        private CategoryDto MapToDto(Category category, int depth)
        {
            var dto = new CategoryDto
            {
                Id = category.Id,
                Name = category.Name,
                ParentId = category.ParentId,
                Data = category.Data,
                Schema = category.Schema,
                IsLatest = category.IsLatest,
                LoadDate = category.LoadDate,
                Children = (depth > 0 && category.Children != null)
                    ? category.Children.Select(child => MapToDto(child, depth - 1)).ToList()
                    : null
            };
            return dto;
        }
    }
}
