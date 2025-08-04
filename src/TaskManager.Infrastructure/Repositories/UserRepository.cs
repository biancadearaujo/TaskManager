using TaskManager.Application.Exceptions;
using TaskManager.Domain.Entities;
using TaskManager.Domain.Repositories;

namespace TaskManager.Infrastructure.Repositories;

public class UserRepository : IUserRepository
{
    private readonly TaskManagerDbContext _context;

    public UserRepository(TaskManagerDbContext context)
    {
        _context = context;
    }
    
    public async Task AddAsync(UserEntity user)
    {
        await _context.Users.AddAsync(user);
        await _context.SaveChangesAsync();
    }
    
    public async Task<UserEntity> GetByIdAsync(Guid id)
    {
        var user = await _context.Users.FindAsync(id);

        if (user == null)
        {
            throw new NotFoundException($"Task with id {id} not found");
        }
        
        return user;
    }
    
    public async Task UpdateAsync(UserEntity userEntity)
    {
        _context.Users.Update(userEntity);
        await _context.SaveChangesAsync();
    }
    
    public async Task DeleteAsync(UserEntity userEntity)
    {
        _context.Users.Remove(userEntity);
        await _context.SaveChangesAsync();
    }
}