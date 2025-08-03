using TaskManager.Domain.Repositories;
using TaskManager.Application.DTOs.TaskManagerDTOs;
using TaskManager.Application.Exceptions;
using TaskManager.Domain.Entities;

namespace TaskManager.Application.Services;

public class TaskService : ITaskService
{
    private readonly ITaskRepository _taskRepository;
    
    public TaskService(ITaskRepository taskRepository)
    {
        _taskRepository = taskRepository;
    }

    public async Task<TaskEntity> CreateAsync(CreateTaskDto createTaskDto)
    {
        var task = new TaskEntity(createTaskDto.Title, createTaskDto.Description);
        await _taskRepository.AddAsync(task);
        
        return task;
    }

    public async Task<TaskEntity> GetByIdAsync(Guid id)
    {
        var task = await _taskRepository.GetByIdAsync(id);
        return task;
    }

    public async Task<IReadOnlyList<TaskEntity>> GetAllAsync()
    {
        var tasks = await _taskRepository.GetAllAsync();
        return tasks;
    }

    public async Task UpdateAsync(Guid id, UpdateTaskDto updateTaskDto)
    {
        var task = await _taskRepository.GetByIdAsync(id);
        
        if (task == null)
        {
            throw new NotFoundException($"Task with id {id} not found");
        }
        
        task.Title = updateTaskDto.Title;
        task.Description = updateTaskDto.Description;
        
        await _taskRepository.UpdateAsync(task);
    }

    public async Task DeleteAsync(Guid id)
    {
        var task = await _taskRepository.GetByIdAsync(id);
        
        if (task == null)
        {
            throw new NotFoundException($"Task with id {id} not found");
        }
        
        await _taskRepository.DeleteAsync(task);
    }
}