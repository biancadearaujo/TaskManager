using System.Data;
using Dapper;
using TaskManager.Application.Exceptions;
using TaskManager.Domain.Entities;
using TaskManager.Domain.Repositories;

namespace TaskManager.Infrastructure.Repositories;

public class TaskRepository : ITaskRepository
    {
        private readonly IDbConnection _dbConnection;

        public TaskRepository(IDbConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }

        public async Task AddAsync(TaskEntity task)
        {
            var sql = @"
    INSERT INTO ""TaskEntity"" 
    (""Id"", ""Title"", ""Description"", ""Status"", ""CompletedAt"", ""UserId"", ""CreatedAt"")
    VALUES 
    (@Id, @Title, @Description, @Status, @CompletedAt, @UserId, @CreatedAt)";
    
            await _dbConnection.ExecuteAsync(sql, new 
            {
                task.Id,
                task.Title,
                task.Description,
                Status = (int)task.Status,
                task.CompletedAt,
                task.UserId,
                task.CreatedAt
            });
        }

    public async Task<TaskEntity> GetByIdAsync(Guid id)
    {
        var sql = @"
        SELECT * 
        FROM ""TaskEntity""
        WHERE ""Id"" = @Id";
        
        var result = await _dbConnection.QuerySingleOrDefaultAsync<TaskEntity>(sql, new { Id = id });
        return result;
    }

    public async Task<IReadOnlyList<TaskEntity>> GetAllAsync(Guid userId)
    {
        var sql = @"
        SELECT * 
        FROM ""TaskEntity""
        WHERE ""UserId"" = @UserId";
        
        var result = await _dbConnection.QueryAsync<TaskEntity>(sql, new { UserId = userId });
        return result.ToList();
    }

    public async Task UpdateAsync(TaskEntity taskEntity)
    {
        var sql = @"
    UPDATE ""TaskEntity"" 
    SET 
        ""Title"" = @Title, 
        ""Description"" = @Description, 
        ""Status"" = @Status,
        ""CompletedAt"" = @CompletedAt
    WHERE ""Id"" = @Id";
    
        var rowsAffected = await _dbConnection.ExecuteAsync(sql, new 
        {
            Title = taskEntity.Title,
            Description = taskEntity.Description,
            Status = (int)taskEntity.Status,
            CompletedAt = taskEntity.CompletedAt,
            Id = taskEntity.Id
        });

        if (rowsAffected == 0)
        {
            throw new NotFoundException($"Task with id {taskEntity.Id} not found in repository.");
        }
    }

    public async Task DeleteAsync(TaskEntity taskEntity)
    {
        var sql = @"
        DELETE FROM ""TaskEntity"" 
        WHERE ""Id"" = @Id";
        
        await _dbConnection.ExecuteAsync(sql, new { Id = taskEntity.Id });
    }
}