namespace EcotrackPlatform.API.Report.Application.Internal.QueryServices;

using EcotrackPlatform.API.Monitoringandcontrol.Domain.Repositories;
using EcotrackPlatform.API.Monitoringandcontrol.Domain.Model.ValueObjects;
using EcotrackPlatform.API.Report.Infrastructure.Services;

public class ReportQueryService
{
    private readonly ITaskRepository _taskRepository;
    private readonly PdfReportGeneratorService _pdfGenerator;

    public ReportQueryService(ITaskRepository taskRepository, PdfReportGeneratorService pdfGenerator)
    {
        _taskRepository = taskRepository;
        _pdfGenerator = pdfGenerator;
    }

    /// <summary>
    /// Genera un reporte en tiempo real del estado de todas las tareas
    /// </summary>
    public async Task<object> GenerateTasksReportAsync()
    {
        // Obtener todas las tareas del repositorio
        var allTasks = await _taskRepository.GetAllAsync();

        // Calcular estadísticas
        var totalTasks = allTasks.Count;
        var completedTasks = allTasks.Count(t => t.Status == TaskStatus.Completed);
        var pendingTasks = allTasks.Count(t => t.Status == TaskStatus.Pending);
        var inProgressTasks = allTasks.Count(t => t.Status == TaskStatus.InProgress);

        // Crear la estructura del reporte con datos reales
        var report = new
        {
            GeneratedAt = DateTime.UtcNow,
            Summary = new
            {
                TotalTasks = totalTasks,
                CompletedTasks = completedTasks,
                PendingTasks = pendingTasks,
                InProgressTasks = inProgressTasks,
                CompletionRate = totalTasks > 0 ? Math.Round((double)completedTasks / totalTasks * 100, 2) : 0
            },
            Tasks = allTasks.Select(t => new
            {
                Id = t.Id,
                Title = t.Title,
                ResponsibleId = t.ResponsibleId,
                Status = t.Status.ToString(),
                StartedAt = t.StartedAt,
                CompletedAt = t.CompletedAt,
                CreatedAt = t.CreatedAt,
                UpdatedAt = t.UpdatedAt
            }).OrderByDescending(t => t.CreatedAt).ToList()
        };

        return await Task.FromResult(report);
    }

    /// <summary>
    /// Genera un reporte en formato PDF del estado de todas las tareas
    /// </summary>
    public async Task<byte[]> GenerateTasksReportPdfAsync()
    {
        // Obtener el reporte en formato JSON
        var reportData = await GenerateTasksReportAsync();
        
        // Generar el PDF usando el servicio
        var pdfBytes = _pdfGenerator.GenerateTasksReportPdf(reportData);
        
        return pdfBytes;
    }
}

