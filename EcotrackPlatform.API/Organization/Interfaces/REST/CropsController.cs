﻿using EcotrackPlatform.API.Organization.Aplication.Services;
using EcotrackPlatform.API.Organization.Domain.Model.Aggregates;
using EcotrackPlatform.API.Organization.Domain.Model.Queries;
using EcotrackPlatform.API.Organization.Interfaces.REST.Resources;
using EcotrackPlatform.API.Organization.Interfaces.REST.Transform;
using Microsoft.AspNetCore.Mvc;

namespace EcotrackPlatform.API.Organization.Interfaces.REST;

[ApiController]
[Route("api/v1/crops")]
public class CropsController : ControllerBase
{
    private readonly ICropCommandService _commandService;
    private readonly ICropQueryService _queryService;

    public CropsController(ICropCommandService commandService, ICropQueryService queryService)
    {
        _commandService = commandService;
        _queryService = queryService;
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateCropResource resource)
    {
        var command = CreateCropCommandFromResourceAssembler.ToCommand(resource);
        
        Crop result = await _commandService.Handle(command);

        var resourceResult = CropResourceFromEntityAssembler.ToResource(result);

        return Ok(resourceResult);
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var query = new GetAllCropsQuery();
        var crops = await _queryService.Handle(query);
        var resources = crops.Select(CropResourceFromEntityAssembler.ToResource);
        return Ok(resources);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var query = new GetCropByIdQuery(id);
        var crop = await _queryService.Handle(query);
        
        if (crop == null)
            return NotFound();
        
        var resource = CropResourceFromEntityAssembler.ToResource(crop);
        return Ok(resource);
    }

    [HttpGet("organization/{organizationId}")]
    public async Task<IActionResult> GetByOrganizationId(int organizationId)
    {
        var query = new GetAllCropsByOrganizationIdQuery(organizationId);
        var crops = await _queryService.Handle(query);
        var resources = crops.Select(CropResourceFromEntityAssembler.ToResource);
        return Ok(resources);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var deleted = await _commandService.Handle(id);
        if (!deleted) return NotFound();
        return NoContent();
    }
}