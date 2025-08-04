using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TaskManager.Domain.Entities;

namespace TaskManager.Infrastructure;

public class TaskManagerDbContext : IdentityDbContext<UserEntity, IdentityRole<Guid>, Guid>
{
    public TaskManagerDbContext(DbContextOptions<TaskManagerDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        //Converte Enum para String.
        modelBuilder.Entity<TaskEntity>()
            .Property(t => t.Status)
            .HasConversion<string>();
        
        //Relacionamento user(One) -> TaskEntity(Many).
        modelBuilder.Entity<TaskEntity>()
            .HasOne(t => t.User)
            .WithMany(u => u.Tasks)
            .HasForeignKey(t => t.UserId)
            .OnDelete(DeleteBehavior.Cascade);
        
        base.OnModelCreating(modelBuilder);
        
    }
}