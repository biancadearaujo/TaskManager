using TaskManager.Domain.Repositories;
using TaskManager.Application.DTOs.TaskManagerDTOs;
using TaskManager.Application.Exceptions;
using TaskManager.Domain.Entities;

namespace TaskManager.Application.Services;

public class TaskService : ITaskService
{
    private readonly ITaskRepository _taskRepository;
    private readonly IUserService _userService;
    
    public TaskService(ITaskRepository taskRepository, IUserService userService)
    {
        _taskRepository = taskRepository;
        _userService = userService;
        
    }

    public async Task<TaskEntity> CreateAsync(CreateTaskDto createTaskDto, Guid currentUserId)
    {
        var user = await _userService.GetByIdAsync(currentUserId);
        
        var task = new TaskEntity(createTaskDto.Title, createTaskDto.Description, currentUserId);
        
        await _taskRepository.AddAsync(task);
        
        return task;
    }

    public async Task<TaskEntity> GetByIdAsync(Guid id, Guid currentUserId)
    {
        
        var task = await _taskRepository.GetByIdAsync(id);
        
        if (task == null)
        {
            throw new NotFoundException($"Task with id {id} not found");
        }
        
        if (task.UserId != currentUserId)
        {
            throw new ForbiddenException("You are not authorized to view this task.");
        }
        
        return task;
    }

    public async Task<IReadOnlyList<TaskEntity>> GetAllAsync(Guid currentUserId)
    {
        var tasks = await _taskRepository.GetAllAsync(currentUserId);
        return tasks;
    }

    public async Task UpdateAsync(Guid id, UpdateTaskDto updateTaskDto, Guid currentUserId)
    {
        var task = await _taskRepository.GetByIdAsync(id);
        
        if (task == null)
        {
            throw new NotFoundException($"Task with id {id} not found");
        }
        
        if (task.UserId != currentUserId)
        {
            throw new ForbiddenException("You are not authorized to update this task.");
        }
        
        task.Title = updateTaskDto.Title;
        task.Description = updateTaskDto.Description;
        
        await _taskRepository.UpdateAsync(task);
    }

    public async Task DeleteAsync(Guid id, Guid currentUserId)
    {
        var task = await _taskRepository.GetByIdAsync(id);
        
        if (task == null)
        {
            throw new NotFoundException($"Task with id {id} not found");
        }
        
        if (task.UserId != currentUserId)
        {
            throw new ForbiddenException("You are not authorized to delete this task.");
        }
        
        await _taskRepository.DeleteAsync(task);
    }
}