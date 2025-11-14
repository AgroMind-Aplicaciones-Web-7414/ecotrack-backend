using Microsoft.AspNetCore.Mvc;
using EcotrackPlatform.API.Monitoringandcontrol.Application.Internal.CommandServices;
using EcotrackPlatform.API.Monitoringandcontrol.Application.Internal.QueryServices;
using EcotrackPlatform.API.Monitoringandcontrol.Interfaces.REST.Resources;
using EcotrackPlatform.API.Monitoringandcontrol.Interfaces.REST.Transform;
using Swashbuckle.AspNetCore.Annotations;
using System.Linq;
using System.Threading.Tasks;
using EcotrackPlatform.API.Monitoringandcontrol.Domain.Model.Entities;
using EcotrackPlatform.API.Monitoringandcontrol.Interfaces.REST.Resources.Requests;

namespace EcotrackPlatform.API.Monitoringandcontrol.Interfaces.REST.Controllers;


[ApiController]
[Route("api/checklists")]
[Tags("Checklists")]
public class ChecklistController : ControllerBase
{
    private readonly CreateChecklistCommandService _createChecklistCommandService;
    private readonly GetChecklistByTaskIdQueryService _getChecklistByTaskIdQueryService;

    public ChecklistController(CreateChecklistCommandService createChecklistCommandService,
        GetChecklistByTaskIdQueryService getChecklistByTaskIdQueryService)
    {
        _createChecklistCommandService = createChecklistCommandService;
        _getChecklistByTaskIdQueryService = getChecklistByTaskIdQueryService;
    }

    [HttpPost]
    [SwaggerOperation(Summary = "Create a new Checklist")]
    public async Task<IActionResult> Create([FromBody] CreateChecklistRequest request)
    {
        var items = request.Items.Select(i => i.Description).ToList();
        var checklistId = await _createChecklistCommandService.Handle(request.TaskId, request.Title, items);
        return CreatedAtAction(nameof(GetByTaskId), new { Id = checklistId }, new { Id = checklistId });
    }

    [HttpGet]
    [SwaggerOperation(Summary = "Get checklists by Task Id")]
    public async Task<IActionResult> GetByTaskId(string taskId)
    {
        var checklist = await _getChecklistByTaskIdQueryService.Handle(taskId);
        if (checklist == null)
        {
            return Ok(new {message = "Checklist not found"});
        }
        return Ok(ChecklistAssembler.ToResource(checklist));
    }
}