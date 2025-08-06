using FluentAssertions;
using Moq;
using TaskManager.Application.DTOs.TaskManagerDTOs;
using TaskManager.Application.Exceptions;
using TaskManager.Application.Services;
using TaskManager.Domain.Entities;
using TaskManager.Domain.Enums;
using TaskManager.Domain.Repositories;

namespace TaskManager.Tests.Unit.Application.Services
{
    public class TaskServiceTests
    {
        private readonly Mock<ITaskRepository> _taskRepositoryMock;
        private readonly Mock<IUserService> _userServiceMock;
        private readonly TaskService _taskService;

        public TaskServiceTests()
        {
            _taskRepositoryMock = new Mock<ITaskRepository>();
            _userServiceMock = new Mock<IUserService>();
            _taskService = new TaskService(_taskRepositoryMock.Object, _userServiceMock.Object);
        }

        [Fact]
        public async Task CreateAsync_ShouldCreateTask_WhenDataIsValid()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var createTaskDto = new CreateTaskDto("Title", "Description");
            var userEntity = new UserEntity
            {
                Id = userId,
                UserName = "Test User",
                Email = "test@email.com",
                CreatedAt = DateTime.UtcNow
            };

            _userServiceMock.Setup(s => s.GetByIdAsync(userId)).ReturnsAsync(userEntity);

            // Act
            var result = await _taskService.CreateAsync(createTaskDto, userId);

            // Assert
            result.Should().NotBeNull();
            result.Title.Should().Be(createTaskDto.Title);
            result.Description.Should().Be(createTaskDto.Description);
            result.UserId.Should().Be(userId);

            _taskRepositoryMock.Verify(r => r.AddAsync(It.IsAny<TaskEntity>()), Times.Once);
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnTask_WhenTaskExistsAndBelongsToUser()
        {
            var taskId = Guid.NewGuid();
            var userId = Guid.NewGuid();
            var task = new TaskEntity("Task Title", "Task Description", userId);

            _taskRepositoryMock.Setup(r => r.GetByIdAsync(taskId)).ReturnsAsync(task);

            var result = await _taskService.GetByIdAsync(taskId, userId);

            result.Should().NotBeNull();
            result.UserId.Should().Be(userId);
            _taskRepositoryMock.Verify(r => r.GetByIdAsync(taskId), Times.Once);
        }

        [Fact]
        public async Task GetByIdAsync_ShouldThrowNotFoundException_WhenTaskDoesNotExist()
        {
            var taskId = Guid.NewGuid();
            var userId = Guid.NewGuid();

            _taskRepositoryMock.Setup(r => r.GetByIdAsync(taskId)).ReturnsAsync((TaskEntity)null);

            Func<Task> act = async () => await _taskService.GetByIdAsync(taskId, userId);

            await act.Should().ThrowAsync<NotFoundException>()
                .WithMessage($"Task with id {taskId} not found");
        }

        [Fact]
        public async Task GetByIdAsync_ShouldThrowForbiddenException_WhenTaskDoesNotBelongToUser()
        {
            var taskId = Guid.NewGuid();
            var userId = Guid.NewGuid();
            var otherUserId = Guid.NewGuid();
            var task = new TaskEntity("Task Title", "Description", otherUserId);

            _taskRepositoryMock.Setup(r => r.GetByIdAsync(taskId)).ReturnsAsync(task);

            Func<Task> act = async () => await _taskService.GetByIdAsync(taskId, userId);

            await act.Should().ThrowAsync<ForbiddenException>()
                .WithMessage("You are not authorized to view this task.");
        }

        [Fact]
        public async Task GetAllAsync_ShouldReturnTasks_ForGivenUserId()
        {
            var userId = Guid.NewGuid();
            var tasks = new List<TaskEntity>
            {
                new TaskEntity("Task 1", "Desc 1", userId),
                new TaskEntity("Task 2", "Desc 2", userId)
            };

            _taskRepositoryMock.Setup(r => r.GetAllAsync(userId)).ReturnsAsync(tasks);

            var result = await _taskService.GetAllAsync(userId);

            result.Should().NotBeNull().And.HaveCount(2);
            _taskRepositoryMock.Verify(r => r.GetAllAsync(userId), Times.Once);
        }

        [Fact]
        public async Task UpdateAsync_ShouldUpdateTask_WhenDataIsValid()
        {
            var taskId = Guid.NewGuid();
            var userId = Guid.NewGuid();
            var task = new TaskEntity("Old Title", "Old Description", userId);
            var updateDto = new UpdateTaskDto
            {
                Title = "New Title",
                Description = "New Description",
                Status = Status.InProgress
            };

            _taskRepositoryMock.Setup(r => r.GetByIdAsync(taskId)).ReturnsAsync(task);

            await _taskService.UpdateAsync(taskId, updateDto, userId);

            task.Title.Should().Be("New Title");
            task.Description.Should().Be("New Description");
            task.Status.Should().Be(Status.InProgress);
            task.CompletedAt.Should().BeNull();

            _taskRepositoryMock.Verify(r => r.UpdateAsync(task), Times.Once);
        }

        [Fact]
        public async Task UpdateAsync_ShouldSetCompletedAt_WhenStatusIsCompleted()
        {
            var taskId = Guid.NewGuid();
            var userId = Guid.NewGuid();
            var task = new TaskEntity("Title", "Description", userId);
            var updateDto = new UpdateTaskDto
            {
                Title = null,
                Description = null,
                Status = Status.Completed
            };

            _taskRepositoryMock.Setup(r => r.GetByIdAsync(taskId)).ReturnsAsync(task);

            await _taskService.UpdateAsync(taskId, updateDto, userId);

            task.Status.Should().Be(Status.Completed);
            task.CompletedAt.Should().NotBeNull();

            _taskRepositoryMock.Verify(r => r.UpdateAsync(task), Times.Once);
        }

        [Fact]
        public async Task UpdateAsync_ShouldThrowNotFoundException_WhenTaskDoesNotExist()
        {
            var taskId = Guid.NewGuid();
            var userId = Guid.NewGuid();
            var updateDto = new UpdateTaskDto
            {
                Title = "Title",
                Description = "Description",
                Status = Status.InProgress
            };

            _taskRepositoryMock.Setup(r => r.GetByIdAsync(taskId)).ReturnsAsync((TaskEntity)null);

            Func<Task> act = async () => await _taskService.UpdateAsync(taskId, updateDto, userId);

            await act.Should().ThrowAsync<NotFoundException>()
                .WithMessage($"Task with id {taskId} not found");
        }

        [Fact]
        public async Task UpdateAsync_ShouldThrowForbiddenException_WhenTaskDoesNotBelongToUser()
        {
            var taskId = Guid.NewGuid();
            var userId = Guid.NewGuid();
            var otherUserId = Guid.NewGuid();
            var task = new TaskEntity("Title", "Description", otherUserId);
            var updateDto = new UpdateTaskDto
            {
                Title = "New Title",
                Description = "New Description",
                Status = Status.InProgress
            };

            _taskRepositoryMock.Setup(r => r.GetByIdAsync(taskId)).ReturnsAsync(task);

            Func<Task> act = async () => await _taskService.UpdateAsync(taskId, updateDto, userId);

            await act.Should().ThrowAsync<ForbiddenException>()
                .WithMessage("You are not authorized to update this task.");
        }

        [Fact]
        public async Task DeleteAsync_ShouldDeleteTask_WhenTaskExistsAndBelongsToUser()
        {
            var taskId = Guid.NewGuid();
            var userId = Guid.NewGuid();
            var task = new TaskEntity("Title", "Description", userId);

            _taskRepositoryMock.Setup(r => r.GetByIdAsync(taskId)).ReturnsAsync(task);

            await _taskService.DeleteAsync(taskId, userId);

            _taskRepositoryMock.Verify(r => r.DeleteAsync(task), Times.Once);
        }

        [Fact]
        public async Task DeleteAsync_ShouldThrowNotFoundException_WhenTaskDoesNotExist()
        {
            var taskId = Guid.NewGuid();
            var userId = Guid.NewGuid();

            _taskRepositoryMock.Setup(r => r.GetByIdAsync(taskId)).ReturnsAsync((TaskEntity)null);

            Func<Task> act = async () => await _taskService.DeleteAsync(taskId, userId);

            await act.Should().ThrowAsync<NotFoundException>()
                .WithMessage($"Task with id {taskId} not found");
        }

        [Fact]
        public async Task DeleteAsync_ShouldThrowForbiddenException_WhenTaskDoesNotBelongToUser()
        {
            var taskId = Guid.NewGuid();
            var userId = Guid.NewGuid();
            var otherUserId = Guid.NewGuid();
            var task = new TaskEntity("Title", "Description", otherUserId);

            _taskRepositoryMock.Setup(r => r.GetByIdAsync(taskId)).ReturnsAsync(task);

            Func<Task> act = async () => await _taskService.DeleteAsync(taskId, userId);

            await act.Should().ThrowAsync<ForbiddenException>()
                .WithMessage("You are not authorized to delete this task.");
        }
    }
}
