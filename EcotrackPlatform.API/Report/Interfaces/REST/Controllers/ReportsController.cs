namespace EcotrackPlatform.API.Report.Interfaces.REST.Controllers;

using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using EcotrackPlatform.API.Report.Application.Internal.CommandServices;
using EcotrackPlatform.API.Report.Application.Internal.QueryServices;
using EcotrackPlatform.API.Report.Domain.Commands;
using EcotrackPlatform.API.Report.Interfaces.REST.Resources;
using EcotrackPlatform.API.Report.Interfaces.REST.Transform;

[ApiController]
[Route("api/reports")]
public class ReportsController : ControllerBase
{
    private readonly ReportCommandService _commandService;
    private readonly ReportQueryService _queryService;

    public ReportsController(ReportCommandService commandService, ReportQueryService queryService)
    {
        _commandService = commandService;
        _queryService = queryService;
    }

    /// <summary>
    /// POST /api/reports/tasks - Solicitar un nuevo reporte de tareas
    /// </summary>
    [HttpPost("tasks")]
    [SwaggerOperation(
        Summary = "Solicitar un nuevo reporte de tareas",
        Description = "Crea una solicitud de reporte de tareas en estado REQUESTED. El contenido se generará posteriormente.",
        OperationId = "RequestTaskReport"
    )]
    [SwaggerResponse(201, "Reporte solicitado exitosamente", typeof(ReportDetailResource))]
    [SwaggerResponse(400, "Datos de solicitud inválidos")]
    public async Task<IActionResult> RequestTaskReport([FromBody] RequestTaskReportResource resource)
    {
        try
        {
            var command = RequestTaskReportCommandFromResourceAssembler.ToCommand(resource);
            var report = await _commandService.HandleRequestTaskReportCommand(command);

            // Obtener el reporte completo con su metadata
            var reportDetail = await _queryService.GetReportByIdAsync(report.Id);
            
            if (reportDetail == null)
            {
                return StatusCode(500, "Error al recuperar el reporte creado");
            }

            var reportResource = ReportDetailResourceFromDtoAssembler.ToResource(reportDetail);
            return CreatedAtAction(nameof(GetReportById), new { reportId = report.Id }, reportResource);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = $"Error al solicitar el reporte: {ex.Message}" });
        }
    }

    /// <summary>
    /// POST /api/reports/{reportId}/generate - Generar el contenido de un reporte
    /// </summary>
    [HttpPost("{reportId}/generate")]
    [SwaggerOperation(
        Summary = "Generar el contenido de un reporte",
        Description = "Genera el contenido JSON del reporte y cambia su estado a GENERATED",
        OperationId = "GenerateReport"
    )]
    [SwaggerResponse(200, "Reporte generado exitosamente", typeof(ReportDetailResource))]
    [SwaggerResponse(404, "Reporte no encontrado")]
    [SwaggerResponse(400, "Error al generar el reporte")]
    public async Task<IActionResult> GenerateReport([FromRoute] int reportId)
    {
        try
        {
            var command = new GenerateReportCommand(reportId);
            var report = await _commandService.HandleGenerateReportCommand(command);

            // Obtener el reporte completo con contenido decodificado
            var reportDetail = await _queryService.GetReportByIdAsync(report.Id);
            
            if (reportDetail == null)
            {
                return NotFound(new { message = $"Reporte con ID {reportId} no encontrado" });
            }

            var reportResource = ReportDetailResourceFromDtoAssembler.ToResource(reportDetail);
            return Ok(reportResource);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = $"Error al generar el reporte: {ex.Message}" });
        }
    }

    /// <summary>
    /// GET /api/reports/{reportId} - Obtener un reporte por ID
    /// </summary>
    [HttpGet("{reportId}")]
    [SwaggerOperation(
        Summary = "Obtener un reporte por ID",
        Description = "Obtiene los detalles completos de un reporte, incluyendo metadata y contenido decodificado como JSON",
        OperationId = "GetReportById"
    )]
    [SwaggerResponse(200, "Reporte encontrado", typeof(ReportDetailResource))]
    [SwaggerResponse(404, "Reporte no encontrado")]
    public async Task<IActionResult> GetReportById([FromRoute] int reportId)
    {
        try
        {
            var reportDetail = await _queryService.GetReportByIdAsync(reportId);
            
            if (reportDetail == null)
            {
                return NotFound(new { message = $"Reporte con ID {reportId} no encontrado" });
            }

            var reportResource = ReportDetailResourceFromDtoAssembler.ToResource(reportDetail);
            return Ok(reportResource);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = $"Error al obtener el reporte: {ex.Message}" });
        }
    }

    /// <summary>
    /// GET /api/reports/profile/{profileId} - Obtener todos los reportes de un perfil
    /// </summary>
    [HttpGet("profile/{profileId}")]
    [SwaggerOperation(
        Summary = "Obtener todos los reportes de un perfil",
        Description = "Obtiene la lista de todos los reportes solicitados por un perfil específico",
        OperationId = "GetReportsByProfileId"
    )]
    [SwaggerResponse(200, "Lista de reportes", typeof(IEnumerable<ReportDetailResource>))]
    public async Task<IActionResult> GetReportsByProfileId([FromRoute] int profileId)
    {
        try
        {
            var reports = await _queryService.GetReportsByProfileIdAsync(profileId);
            var resources = reports.Select(ReportDetailResourceFromDtoAssembler.ToResource);
            return Ok(resources);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = $"Error al obtener los reportes: {ex.Message}" });
        }
    }
}