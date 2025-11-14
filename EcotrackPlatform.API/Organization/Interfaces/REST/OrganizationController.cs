using EcotrackPlatform.API.Organization.Aplication.Services;
using EcotrackPlatform.API.Organization.Interfaces.REST.Resources;
using EcotrackPlatform.API.Organization.Interfaces.REST.Transform;
using Microsoft.AspNetCore.Mvc;

namespace EcotrackPlatform.API.Organization.Interfaces.REST;

[ApiController]
[Route("api/v1/organizations")]
public class OrganizationsController : ControllerBase
{
    private readonly IOrganizationCommandService _commandService;

    public OrganizationsController(IOrganizationCommandService commandService)
    {
        _commandService = commandService;
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateOrganizationResource resource)
    {
        var command = CreateOrganizationCommandFromResourceAssembler.ToCommand(resource);
        var result = await _commandService.Handle(command);

        var resourceResult = OrganizationResourceFromEntityAssembler.ToResource(result);

        return Ok(resourceResult);
    }
}