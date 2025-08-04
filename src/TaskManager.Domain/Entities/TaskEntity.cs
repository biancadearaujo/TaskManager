using TaskManager.Domain.Enums;

namespace TaskManager.Domain.Entities;

public class TaskEntity
{
    public Guid Id {get; private set; }
    public string Title {get; set;}
    public string Description {get; set;}
    public Status Status {get; set;}
    public DateTime CreatedAt {get; set;}
    public DateTime? CompletedAt {get; set;}
    public Guid UserId { get; set; }
    public UserEntity User { get; set; }

    public TaskEntity(string title, string description, Guid userId)
    {
        Id = Guid.NewGuid();
        Title = title;
        Description = description;
        Status = Status.Pending;
        CreatedAt = DateTime.UtcNow;
        UserId = userId;
    }

    public TaskEntity()
    {
    }

    public void CompleteTask()
    {
        if (Status != Status.Completed)
        {
            Status = Status.Completed;
            CompletedAt = DateTime.UtcNow;
        }
    }
}