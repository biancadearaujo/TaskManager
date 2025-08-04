using TaskManager.Application.DTOs.UserDTOs;
using TaskManager.Domain.Entities;

namespace TaskManager.Application.Services;

public interface IUserService
{
    Task<UserEntity> CreateAsync(CreateUserDto createUserDto);
    Task<UserEntity> GetByIdAsync(Guid id);
    Task UpdateAsync(Guid id, UpdateUserDto updateUserDto);
    Task DeleteAsync(Guid id);
}