using TaskManager.Domain.Enums;
using System.Text.Json.Serialization;

namespace TaskManager.Application.DTOs.TaskManagerDTOs;

public class UpdateTaskDto
{
    public string? Title { get; set; }
    public string? Description { get; set; }
    
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public Status? Status { get; set; }
    
    public UpdateTaskDto(string title, string description, Status? status)
    {
        Title = title;
        Description = description;
        Status = status;
    }
    
    public UpdateTaskDto() { }
}