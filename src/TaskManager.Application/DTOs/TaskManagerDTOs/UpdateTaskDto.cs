using TaskManager.Domain.Enums;
using System.Text.Json.Serialization;

namespace TaskManager.Application.DTOs.TaskManagerDTOs;

public class UpdateTaskDto
{
    public string? Title { get; init; }
    public string? Description { get; init; }
    
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public Status? Status { get; init; }
}