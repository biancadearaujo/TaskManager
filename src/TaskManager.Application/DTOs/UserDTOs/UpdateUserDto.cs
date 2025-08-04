namespace TaskManager.Application.DTOs.UserDTOs;

public record UpdateUserDto(
    string Name,
    string Email,
    string Password);