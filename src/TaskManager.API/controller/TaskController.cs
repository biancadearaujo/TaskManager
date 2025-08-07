using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskManager.Application.DTOs.TaskManagerDTOs;
using TaskManager.Application.Exceptions;
using TaskManager.Application.Services;
using TaskManager.Domain.Enums;

namespace TaskManager.API.controller;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class TaskController : ControllerBase
{
    private readonly ITaskService _taskService;

    public TaskController(ITaskService taskService)
    {
        _taskService = taskService;
    }
    
    private Guid GetCurrentUserId()
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(userIdClaim))
        {
            return Guid.Empty;
        }
        return Guid.Parse(userIdClaim);
    }

        [HttpPost]
        public async Task<IActionResult> CreateTask([FromBody] CreateTaskDto createTaskDto)
        {
            var userId = GetCurrentUserId();
            var task = await _taskService.CreateAsync(createTaskDto, userId);
            return CreatedAtAction(nameof(GetTaskById), new { id = task.Id }, task);
        }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetTaskById(Guid id)
    {
        try
        {
            var userId = GetCurrentUserId();
            var task = await _taskService.GetByIdAsync(id, userId);
            return Ok(task);
        }
        catch (NotFoundException e)
        {
            return NotFound(new { error = e.Message });
        }
        catch (ForbiddenException  e)
        {
            return Forbid(e.Message);
        }
    }

    [HttpGet]
    public async Task<IActionResult> GetTasks()
    {
        var userId = GetCurrentUserId();
        var task = await _taskService.GetAllAsync(userId);
        return Ok(task);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateTask(Guid id, [FromBody] UpdateTaskDto updateTaskDto)
    {
        try
        {
            var userId = GetCurrentUserId();
            await _taskService.UpdateAsync(id, updateTaskDto, userId);
            var updatedTask = await _taskService.GetByIdAsync(id, userId);
            return Ok(updatedTask);
        }
        catch (NotFoundException ex)
        {
            return NotFound(new { error = ex.Message });
        }
        catch (ForbiddenException e)
        {
            return Forbid(e.Message );
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteTask(Guid id)
    {
        try
        {
            var userId = GetCurrentUserId();
            await _taskService.DeleteAsync(id, userId);
            return NoContent();
        }
        catch (NotFoundException ex)
        {
            return NotFound(new { error = ex.Message });
        }
        catch (ForbiddenException e)
        {
            return Forbid(e.Message );
        }
    }
}