using Refit;
using HierarchicalDirectory.Models;

namespace HierarchicalDirectory.RefitClient
{
    public interface ICategoryApi
    {
        [Get("api/category")]
        Task<IEnumerable<CategoryDto>> GetAllAsync(int? depth = null, string? search = null);

        [Get("api/category/{id}")]
        Task<CategoryDto> GetByIdAsync(string id, bool includeChildren = false);

        [Post("api/category")]
        Task<CategoryDto> CreateAsync([Body] CategoryDto dto);

        [Put("api/category/{id}")]
        Task<CategoryDto> UpdateAsync(string id, [Body] CategoryDto dto);

        [Delete("api/category/{id}")]
        Task DeleteAsync(string id);
    }
}
