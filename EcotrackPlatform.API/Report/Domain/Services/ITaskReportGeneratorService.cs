namespace EcotrackPlatform.API.Report.Domain.Services;

using EcotrackPlatform.API.Report.Domain.Model;

/// <summary>
/// Servicio de dominio para generar reportes de tareas
/// </summary>
public interface ITaskReportGeneratorService
{
    /// <summary>
    /// Genera el contenido del reporte en formato JSON
    /// </summary>
    Task<string> GenerateReportJsonAsync(Report report);
}

