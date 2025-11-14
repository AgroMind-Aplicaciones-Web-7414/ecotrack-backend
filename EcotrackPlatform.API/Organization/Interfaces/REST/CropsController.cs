using EcotrackPlatform.API.Organization.Aplication.Services;
using EcotrackPlatform.API.Organization.Domain.Model.Aggregates;
using EcotrackPlatform.API.Organization.Interfaces.REST.Resources;
using EcotrackPlatform.API.Organization.Interfaces.REST.Transform;
using Microsoft.AspNetCore.Mvc;

namespace EcotrackPlatform.API.Organization.Interfaces.REST;

[ApiController]
[Route("api/v1/crops")]
public class CropsController : ControllerBase
{
    private readonly ICropCommandService _commandService;

    public CropsController(ICropCommandService commandService)
    {
        _commandService = commandService;
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateCropResource resource)
    {
        var command = CreateCropCommandFromResourceAssembler.ToCommand(resource);
        
        Crop result = await _commandService.Handle(command);

        var resourceResult = CropResourceFromEntityAssembler.ToResource(result);

        return Ok(resourceResult);
    }
}