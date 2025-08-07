using MelkYab.Backend.Data.Dtos;
using MelkYab.Backend.Data.Tables;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace MelkYab.Backend.Data.Interfaces
{
    public interface IService<T> where T : BaseEntity
    {
        Task<T?> GetByIdAsync(int id);
        Task<IEnumerable<T>> GetAllAsync();
        Task<T> CreateAsync(T entity);
        Task<T> UpdateAsync(T entity);
        Task<bool> DeleteAsync(int id);
    }
}