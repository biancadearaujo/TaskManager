using TaskManager.Domain.Entities;

namespace TaskManager.Domain.Repositories;

public interface IUserRepository
{
    Task AddAsync(UserEntity user);
    Task UpdateAsync(UserEntity user);
    Task DeleteAsync(UserEntity user);
    Task<UserEntity> GetByIdAsync(Guid id);
}