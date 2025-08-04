using Microsoft.AspNetCore.Identity;

namespace TaskManager.Domain.Entities;

public class UserEntity : IdentityUser<Guid>
{
    public DateTime CreatedAt { get; set; }
    public ICollection<TaskEntity> Tasks { get; set; }

    public UserEntity() : base()
    {
        Id = Guid.NewGuid();
    }
}