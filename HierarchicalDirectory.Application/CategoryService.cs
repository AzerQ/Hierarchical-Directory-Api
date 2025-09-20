using System.Collections.Generic;
using System.Threading.Tasks;
using HierarchicalDirectory.Domain;

namespace HierarchicalDirectory.Application
{
    public class CategoryService : ICategoryService
    {
        // TODO: Inject repository and implement business logic
        public Task<IEnumerable<CategoryDto>> GetAllAsync(int? depth = null, string? search = null) => Task.FromResult<IEnumerable<CategoryDto>>(new List<CategoryDto>());
        public Task<CategoryDto> GetByIdAsync(string id, bool includeChildren = false) => Task.FromResult<CategoryDto>(null);
        public Task<IEnumerable<CategoryDto>> GetBatchAsync(IEnumerable<string> ids, bool includeChildren = false) => Task.FromResult<IEnumerable<CategoryDto>>(new List<CategoryDto>());
        public Task<CategoryDto> CreateAsync(CategoryDto dto) => Task.FromResult<CategoryDto>(null);
        public Task<CategoryDto> UpdateAsync(string id, CategoryDto dto) => Task.FromResult<CategoryDto>(null);
        public Task DeleteAsync(string id) => Task.CompletedTask;
        public Task<CategoryDto> CreateLeafVersionAsync(string parentId, CategoryDto dto) => Task.FromResult<CategoryDto>(null);
        public Task<(bool valid, List<string> errors)> ValidateAsync(string id, object data) => Task.FromResult((true, new List<string>()));
    }
}
