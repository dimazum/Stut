using System.Collections.Generic;
using System.Threading.Tasks;
using StopStatAuth_6_0.Entities.Base;

namespace stutvds.DAL.Contracts
{
    public interface IBaseRepository<T> where T : Entity
    {
        Task<IReadOnlyList<T>> GetAllAsync();
        Task<T> GetByIdAsync(int id);
        Task<T> AddAsync(T entity);
        Task UpdateAsync(T entity);
        Task DeleteAsync(T entity);
    }
}