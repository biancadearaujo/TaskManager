using Microsoft.EntityFrameworkCore;
using TaskManager.Application.Exceptions;
using TaskManager.Domain.Entities;
using TaskManager.Domain.Repositories;

namespace TaskManager.Infrastructure.Repositories;

public class TaskRepository : ITaskRepository
    {
    private readonly TaskManagerDbContext _context;

    public TaskRepository(TaskManagerDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(TaskEntity task)
    {
        await _context.Tasks.AddAsync(task);
        await _context.SaveChangesAsync();
    }

    public async Task<TaskEntity> GetByIdAsync(Guid id)
    {
        var task = await _context.Tasks.FindAsync(id);

        if (task == null)
        {
            throw new NotFoundException($"Task with id {id} not found");
        }
        
        return task;
    }

    public async Task<IReadOnlyList<TaskEntity>> GetAllAsync()
    {
        return await _context.Tasks.ToListAsync();
    }

    public async Task UpdateAsync(TaskEntity taskEntity)
    {
        _context.Tasks.Update(taskEntity);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(TaskEntity taskEntity)
    {
        _context.Tasks.Remove(taskEntity);
        await _context.SaveChangesAsync();
    }
}