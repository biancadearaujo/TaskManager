using TaskManager.Application.DTOs.TaskManagerDTOs;
using TaskManager.Domain.Entities;

namespace TaskManager.Application.Services;

public interface ITaskService
{
    Task<TaskEntity> CreateAsync(CreateTaskDto createTaskDto);
    Task<TaskEntity> GetByIdAsync(Guid id);
    Task<IReadOnlyList<TaskEntity>> GetAllAsync();
    Task UpdateAsync(Guid id, UpdateTaskDto updateTaskDto);
    Task DeleteAsync(Guid id);
}