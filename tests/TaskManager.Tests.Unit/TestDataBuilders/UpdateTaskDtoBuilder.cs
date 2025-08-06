using TaskManager.Application.DTOs.TaskManagerDTOs;
using TaskManager.Domain.Enums;

namespace TaskManager.Tests.Unit.TestDataBuilders
{
    public class UpdateTaskDtoBuilder
    {
        private string _title = "Updated Title";
        private string _description = "Updated Description";
        private Status? _status = Status.InProgress;

        public UpdateTaskDtoBuilder WithTitle(string title) { _title = title; return this; }
        public UpdateTaskDtoBuilder WithDescription(string description) { _description = description; return this; }
        public UpdateTaskDtoBuilder WithStatus(Status? status) { _status = status; return this; }

        public UpdateTaskDto Build()
        {
            return new UpdateTaskDto
            {
                Title = _title,
                Description = _description,
                Status = _status
            };
        }
    }
}