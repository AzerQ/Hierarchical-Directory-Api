using System.Collections.Generic;
using System.Threading.Tasks;

namespace HierarchicalDirectory.Domain
{
    /// <summary>
    /// Интерфейс репозитория для базовых операций над сущностями.
    /// </summary>
    public interface IRepository<T>
    {
        /// <summary>Получить сущность по идентификатору</summary>
        Task<T> GetByIdAsync(string id);
        /// <summary>Получить все сущности</summary>
        Task<IEnumerable<T>> GetAllAsync();
        /// <summary>Добавить новую сущность</summary>
        Task AddAsync(T entity);
        /// <summary>Обновить существующую сущность</summary>
        Task UpdateAsync(T entity);
        /// <summary>Удалить сущность по идентификатору</summary>
        Task DeleteAsync(string id);
    }
}
