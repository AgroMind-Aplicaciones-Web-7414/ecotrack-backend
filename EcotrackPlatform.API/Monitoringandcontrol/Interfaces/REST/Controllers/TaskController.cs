using Microsoft.AspNetCore.Mvc;
using EcotrackPlatform.API.Monitoringandcontrol.Application.Internal.CommandServices;
using EcotrackPlatform.API.Monitoringandcontrol.Application.Internal.QueryServices;
using EcotrackPlatform.API.Monitoringandcontrol.Interfaces.REST.Transform;
using Swashbuckle.AspNetCore.Annotations;
using EcotrackPlatform.API.Monitoringandcontrol.Interfaces.REST.Resources.Requests;

namespace EcotrackPlatform.API.Monitoringandcontrol.Interfaces.REST.Controllers;

[ApiController]
[Route("api/tasks")]
[Tags("Tasks")]
public class TaskController : ControllerBase
{
    private readonly CreateTaskCommandService _createTaskCommandService;
    private readonly UpdateTaskStatusCommandService _updateTaskStatusCommandService;
    private readonly GetTasksQueryService _getTasksQueryService;

    public TaskController(CreateTaskCommandService createTaskCommandService,
        UpdateTaskStatusCommandService updateTaskStatusCommandService, GetTasksQueryService getTasksQueryService)
    {
        _createTaskCommandService = createTaskCommandService;
        _updateTaskStatusCommandService = updateTaskStatusCommandService;
        _getTasksQueryService = getTasksQueryService;
    }

    [HttpPost]
    [SwaggerOperation(Summary = "Create a new task")]
    public async Task<IActionResult> CreateTask([FromBody] CreateTaskRequest request)
    {
        var taskId = await _createTaskCommandService.Handle(request.Title, request.ResponsibleId);
        var tasks = await _getTasksQueryService.Handle();
        var task = tasks.Find(t => t.Id == taskId);
        return CreatedAtAction(nameof(GetById), new {taskId}, TaskAssembler.ToCreatedResource(task!));
    }

    [HttpPatch("{taskId}/status")]
    [SwaggerOperation(Summary = "Update a task's status")]
    public async Task<IActionResult> UpdateStatus(int taskId, [FromBody] UpdateStatusRequest request)
    {
        await _updateTaskStatusCommandService.Handle(taskId, request.Status);
        return Ok(new { message = "Status Updated" });
    }

    [HttpGet]
    [SwaggerOperation(Summary = "Get tasks by Status")]
    public async Task<IActionResult> GetTasksByStatus([FromQuery]string? status = null)
    {
        var tasks = await _getTasksQueryService.Handle();
        if (!string.IsNullOrEmpty(status))
        {
            if (Enum.TryParse<Domain.Model.ValueObjects.TaskStatus>(status, true, out var statusEnum))
            {
                tasks = tasks.Where(t => t.Status == statusEnum).ToList();
            }
            else
            {
                return BadRequest(new { message = "Invalid status" });
            }
        }
        return Ok(tasks.Select(TaskAssembler.ToResource));
    }

    [HttpGet("{taskId:int}")]
    [SwaggerOperation(Summary = "Get task by id")]
    public async Task<IActionResult> GetById(int taskId)
    {
        var tasks = await _getTasksQueryService.Handle();
        var task = tasks.Find(t => t.Id == taskId);
        if (task == null)
        {
            return NotFound();
        }
        return Ok(TaskAssembler.ToResource(task));
    }
}