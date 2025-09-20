using System.Collections.Generic;
using System.Threading.Tasks;

namespace HierarchicalDirectory.Application
{
    public interface ICategoryService
    {
        Task<IEnumerable<CategoryDto>> GetAllAsync(int? depth = null, string? search = null);
        Task<CategoryDto> GetByIdAsync(string id, bool includeChildren = false);
        Task<IEnumerable<CategoryDto>> GetBatchAsync(IEnumerable<string> ids, bool includeChildren = false);
        Task<CategoryDto> CreateAsync(CategoryDto dto);
        Task<CategoryDto> UpdateAsync(string id, CategoryDto dto);
        Task DeleteAsync(string id);
        Task<CategoryDto> CreateLeafVersionAsync(string parentId, CategoryDto dto);
        Task<(bool valid, List<string> errors)> ValidateAsync(string id, object data);
    }
}
