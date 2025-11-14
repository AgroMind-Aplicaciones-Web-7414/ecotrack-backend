using EcotrackPlatform.API.Monitoringandcontrol.Application.Internal.CommandServices;
using EcotrackPlatform.API.Monitoringandcontrol.Application.Internal.QueryServices;
using EcotrackPlatform.API.Monitoringandcontrol.Interfaces.REST.Resources.Requests;
using EcotrackPlatform.API.Monitoringandcontrol.Interfaces.REST.Transform;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace EcotrackPlatform.API.Monitoringandcontrol.Interfaces.REST.Controllers;

[ApiController]
[Route("api/logbooks")]
[Tags("Logbooks")]

public class LogbookController : ControllerBase
{
    private readonly CreateLogbookCommandService _createLogbookCommandService;
    private readonly GetLogbookQueryService _getLogbookQueryService;

    public LogbookController(CreateLogbookCommandService createLogbookCommandService,
        GetLogbookQueryService getLogbookQueryService)
    {
        _createLogbookCommandService = createLogbookCommandService;
        _getLogbookQueryService = getLogbookQueryService;
    }

    [HttpPost]
    [SwaggerOperation(Summary = "Creates a new logbook")]
    public async Task<IActionResult> Create([FromBody] CreateLogbookRequest request)
    {
        var logbookId = await _createLogbookCommandService.Handle(
            request.Activity, request.Duration, request.Volume, request.Evident);
        var logbooks = await _getLogbookQueryService.Handle();
        var logbook = logbooks.Find(x => x.Id == logbookId);
        return CreatedAtAction(nameof(GetById), new { logbookId }, logbook);
    }
    
    [HttpGet("{logbookId:int}")]
    [SwaggerOperation(Summary = "Get logbook by id")]
    public async Task<IActionResult> GetById(int logbookId)
    {
        var logbooks = await _getLogbookQueryService.Handle();
        var logbook = logbooks.Find(x => x.Id == logbookId);
        if (logbook == null)
        {
            return NotFound();
        }
        return Ok(LogbookAssembler.ToResource(logbook));
    }
    
    [HttpGet]
    [SwaggerOperation(Summary = "Get all logbooks")]
    public async Task<IActionResult> GetAllAsync()
    {
        var logbooks = await _getLogbookQueryService.Handle();
        return Ok(logbooks.Select(LogbookAssembler.ToResource));
    }
}