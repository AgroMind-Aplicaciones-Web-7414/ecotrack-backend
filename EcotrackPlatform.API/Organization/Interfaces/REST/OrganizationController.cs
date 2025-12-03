﻿using EcotrackPlatform.API.Organization.Aplication.Services;
using EcotrackPlatform.API.Organization.Domain.Model.Queries;
using EcotrackPlatform.API.Organization.Interfaces.REST.Resources;
using EcotrackPlatform.API.Organization.Interfaces.REST.Transform;
using Microsoft.AspNetCore.Mvc;

namespace EcotrackPlatform.API.Organization.Interfaces.REST;

[ApiController]
[Route("api/v1/organizations")]
public class OrganizationsController : ControllerBase
{
    private readonly IOrganizationCommandService _commandService;
    private readonly IOrganizationQueryService _queryService;

    public OrganizationsController(IOrganizationCommandService commandService, IOrganizationQueryService queryService)
    {
        _commandService = commandService;
        _queryService = queryService;
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateOrganizationResource resource)
    {
        var command = CreateOrganizationCommandFromResourceAssembler.ToCommand(resource);
        var result = await _commandService.Handle(command);

        var resourceResult = OrganizationResourceFromEntityAssembler.ToResource(result);

        return Ok(resourceResult);
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var query = new GetAllOrganizationsQuery();
        var organizations = await _queryService.Handle(query);
        var resources = organizations.Select(OrganizationResourceFromEntityAssembler.ToResource);
        return Ok(resources);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var query = new GetOrganizationByIdQuery(id);
        var organization = await _queryService.Handle(query);
        
        if (organization == null)
            return NotFound();
        
        var resource = OrganizationResourceFromEntityAssembler.ToResource(organization);
        return Ok(resource);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var deleted = await _commandService.Handle(id);
        if (!deleted) return NotFound();
        return NoContent();
    }
}