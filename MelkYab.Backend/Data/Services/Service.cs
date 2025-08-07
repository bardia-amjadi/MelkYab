using MelkYab.Backend.Data.Dtos;
using MelkYab.Backend.Data.Tables;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace MelkYab.Backend.Data.Services
{
    public class Service<T> : IService<T> where T : BaseEntity
    {
        protected readonly IUnitOfWork _unitOfWork;
        protected readonly IRepository<T> _repository;

        public Service(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _repository = unitOfWork.Repository<T>();
        }

        public async Task<T?> GetByIdAsync(int id) => await _repository.GetByIdAsync(id);

        public async Task<IEnumerable<T>> GetAllAsync() => await _repository.GetAllAsync();

        public async Task<T> CreateAsync(T entity)
        {
            await _repository.AddAsync(entity);
            await _unitOfWork.SaveChangesAsync();
            return entity;
        }

        public async Task<T> UpdateAsync(T entity)
        {
            _repository.Update(entity);
            await _unitOfWork.SaveChangesAsync();
            return entity;
        }   

        public async Task<bool> DeleteAsync(int id)
        {
            var entity = await _repository.GetByIdAsync(id);
            if (entity == null) return false;

            _repository.Remove(entity);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }
    }
}