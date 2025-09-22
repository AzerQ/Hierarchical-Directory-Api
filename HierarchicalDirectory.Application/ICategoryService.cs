using System.Collections.Generic;
using System.Threading.Tasks;

using HierarchicalDirectory.Models;
namespace HierarchicalDirectory.Application
{
    /// <summary>
    /// Интерфейс сервиса для работы с категориями.
    /// </summary>
    public interface ICategoryService
    {
        /// <summary>Получить все категории</summary>
        Task<IEnumerable<CategoryDto>> GetAllAsync(int? depth = null, string? search = null);
        /// <summary>Получить категорию по идентификатору</summary>
        Task<CategoryDto> GetByIdAsync(string id, bool includeChildren = false);
        /// <summary>Получить несколько категорий по списку идентификаторов</summary>
        Task<IEnumerable<CategoryDto>> GetBatchAsync(IEnumerable<string> ids, bool includeChildren = false);
        /// <summary>Создать новую категорию</summary>
        Task<CategoryDto> CreateAsync(CategoryDto dto);
        /// <summary>Обновить существующую категорию</summary>
        Task<CategoryDto> UpdateAsync(string id, CategoryDto dto);
        /// <summary>Удалить категорию по идентификатору</summary>
        Task DeleteAsync(string id);
        /// <summary>Создать новую версию листовой категории</summary>
        Task<CategoryDto> CreateLeafVersionAsync(string parentId, CategoryDto dto);
        /// <summary>Валидировать данные категории по схеме</summary>
        Task<(bool valid, List<string> errors)> ValidateAsync(string id, object data);
    }
}
