namespace EcotrackPlatform.API.Report.Interfaces.REST.Controllers;

using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using EcotrackPlatform.API.Report.Application.Internal.QueryServices;

[ApiController]
[Route("api/reports")]
public class ReportsController : ControllerBase
{
    private readonly ReportQueryService _queryService;

    public ReportsController(ReportQueryService queryService)
    {
        _queryService = queryService;
    }

    /// <summary>
    /// GET /api/reports - Generar reporte del estado de todas las tareas (JSON)
    /// </summary>
    [HttpGet]
    [SwaggerOperation(
        Summary = "Generar reporte del estado de todas las tareas (JSON)",
        Description = "Genera y retorna un reporte en tiempo real con estadísticas y detalles de todas las tareas creadas en el sistema en formato JSON",
        OperationId = "GenerateTasksReport"
    )]
    [SwaggerResponse(200, "Reporte generado exitosamente")]
    public async Task<IActionResult> GenerateTasksReport()
    {
        try
        {
            var report = await _queryService.GenerateTasksReportAsync();
            return Ok(report);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = $"Error al generar el reporte: {ex.Message}" });
        }
    }

    /// <summary>
    /// GET /api/reports/pdf - Generar reporte del estado de todas las tareas (PDF)
    /// </summary>
    [HttpGet("pdf")]
    [SwaggerOperation(
        Summary = "Generar reporte del estado de todas las tareas (PDF)",
        Description = "Genera y descarga un reporte en formato PDF con estadísticas y detalles de todas las tareas creadas en el sistema",
        OperationId = "GenerateTasksReportPdf"
    )]
    [SwaggerResponse(200, "Reporte PDF generado exitosamente")]
    public async Task<IActionResult> GenerateTasksReportPdf()
    {
        try
        {
            var pdfBytes = await _queryService.GenerateTasksReportPdfAsync();
            var fileName = $"Reporte_Tareas_{DateTime.Now:yyyyMMdd_HHmmss}.pdf";
            
            // Retornar el PDF como FileContentResult
            return File(pdfBytes, "application/pdf", fileName);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = $"Error al generar el reporte PDF: {ex.Message}" });
        }
    }
}