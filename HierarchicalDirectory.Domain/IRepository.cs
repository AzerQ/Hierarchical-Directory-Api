using System.Collections.Generic;
using System.Threading.Tasks;

namespace HierarchicalDirectory.Domain
{
    public interface IRepository<T>
    {
        Task<T> GetByIdAsync(string id);
        Task<IEnumerable<T>> GetAllAsync();
        Task AddAsync(T entity);
        Task UpdateAsync(T entity);
        Task DeleteAsync(string id);
    }
}
