using TaskManager.Application.DTOs.TaskManagerDTOs;
using TaskManager.Domain.Entities;

namespace TaskManager.Application.Services;

public interface ITaskService
{
    Task<TaskEntity> CreateAsync(CreateTaskDto createTaskDto, Guid currentUserId);
    Task<TaskEntity> GetByIdAsync(Guid id, Guid currentUserId);
    Task<IReadOnlyList<TaskEntity>> GetAllAsync(Guid userId);
    Task UpdateAsync(Guid id, UpdateTaskDto updateTaskDto, Guid currentUserId);
    Task DeleteAsync(Guid id, Guid currentUserId);
}