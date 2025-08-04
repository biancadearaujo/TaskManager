using Microsoft.AspNetCore.Identity;
using TaskManager.Application.DTOs.UserDTOs;
using TaskManager.Application.Exceptions;
using TaskManager.Domain.Entities;
using TaskManager.Domain.Repositories;

namespace TaskManager.Application.Services;

public class UserService : IUserService
{
    private readonly UserManager<UserEntity> _userManager;

    public UserService(UserManager<UserEntity> userManager)
    {
        _userManager = userManager;
    }

    public async Task<UserEntity> CreateAsync(CreateUserDto createUserDto)
    {
        var user = new UserEntity
        {
            UserName = createUserDto.Email,
            Email = createUserDto.Email,
            CreatedAt = DateTime.UtcNow
        };
        
        var result = await _userManager.CreateAsync(user, createUserDto.Password);

        
        if (result.Succeeded)
        {
            return user;
        }
        else
        {
            throw new Exception($"Falha ao criar usuário");
        }
    }

    public async Task<UserEntity> GetByIdAsync(Guid id)
    {
        var user = await _userManager.FindByIdAsync(id.ToString());

        if (user == null)
        {
            throw new NotFoundException($"User with id {id} not found");
        }
        
        return user;
    }

    public async Task UpdateAsync(Guid id, UpdateUserDto updateUserDto)
    {
        var user = await _userManager.FindByIdAsync(id.ToString());

        if (user == null)
        {
            throw new NotFoundException($"User with id {id} not found");
        }
        user.Email = updateUserDto.Email;
        
        if (!string.IsNullOrEmpty(updateUserDto.Password))
        {
            var newPasswordHash = _userManager.PasswordHasher.HashPassword(user, updateUserDto.Password);
            user.PasswordHash = newPasswordHash;
        }
        
        var result = await _userManager.UpdateAsync(user);

        if (!result.Succeeded)
        {
            throw new Exception($"Falha ao atualizar usuário");
        }
    }

    public async Task DeleteAsync(Guid id)
    {
        var user = await _userManager.FindByIdAsync(id.ToString());

        if (user == null)
        {
            throw new NotFoundException($"User with id {id} not found");
        }
        
        await _userManager.DeleteAsync(user);
    }
}