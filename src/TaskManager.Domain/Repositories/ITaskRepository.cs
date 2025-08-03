using TaskManager.Domain.Entities;

namespace TaskManager.Domain.Repositories;

public interface ITaskRepository
{
    Task AddAsync(TaskEntity taskEntity);
    Task<TaskEntity> GetByIdAsync(Guid id);
    
    Task<IReadOnlyList<TaskEntity>> GetAllAsync();
    
    Task UpdateAsync(TaskEntity taskEntity);
    
    Task DeleteAsync(TaskEntity taskEntity);
}