using TaskManager.Domain.Entities;
using TaskManager.Domain.Enums;

namespace TaskManager.Tests.Unit.TestDataBuilders
{
    public class TaskEntityBuilder
    {
        private Guid _id = Guid.NewGuid();
        private string _title = "Default Task";
        private string _description = "Default Description";
        private Guid _userId = Guid.NewGuid();
        private Status _status = Status.Pending;
        private DateTime? _completedAt = null;

        public TaskEntityBuilder WithId(Guid id) { _id = id; return this; }
        public TaskEntityBuilder WithTitle(string title) { _title = title; return this; }
        public TaskEntityBuilder WithDescription(string description) { _description = description; return this; }
        public TaskEntityBuilder WithUserId(Guid userId) { _userId = userId; return this; }
        public TaskEntityBuilder WithStatus(Status status) { _status = status; return this; }
        public TaskEntityBuilder WithCompletedAt(DateTime? completedAt) { _completedAt = completedAt; return this; }

        public TaskEntity Build()
        {
            var task = new TaskEntity(_title, _description, _userId);
            task.Status = _status;
            task.CompletedAt = _completedAt;
            return task;
        }
    }
}