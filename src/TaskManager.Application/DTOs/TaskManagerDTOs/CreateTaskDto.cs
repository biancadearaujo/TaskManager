namespace TaskManager.Application.DTOs.TaskManagerDTOs;

public record CreateTaskDto(
    string Title,
    string Description
        );