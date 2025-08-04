namespace TaskManager.Application.DTOs.UserDTOs;

public record CreateUserDto(
    string Name,
    string Email,
    string Password);