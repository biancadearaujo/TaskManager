using TaskManager.Domain.Enums;
using System.Text.Json.Serialization;

namespace TaskManager.Application.DTOs.TaskManagerDTOs;

public class UpdateTaskDto
{
    public string? Title { get; init; }
    public string? Description { get; init; }
    
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public Status? Status { get; init; }
    
    public UpdateTaskDto(string title, string description, Status? status)
    {
        Title = title;
        Description = description;
        Status = status;
    }
    
    public UpdateTaskDto() { }
}