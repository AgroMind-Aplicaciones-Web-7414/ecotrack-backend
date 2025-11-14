namespace EcotrackPlatform.API.Report.Application.Internal.CommandServices;

using System.Text;
using EcotrackPlatform.API.Report.Domain.Commands;
using EcotrackPlatform.API.Report.Domain.Repositories;
using EcotrackPlatform.API.Report.Domain.Model;
using EcotrackPlatform.API.Report.Domain.Model.ValueObjects;
using EcotrackPlatform.API.Report.Domain.Services;
using EcotrackPlatform.API.Shared.Domain.Repositories;

public class ReportCommandService
{
    private readonly IReportRepository _reportRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ITaskReportGeneratorService _taskReportGenerator;

    public ReportCommandService(
        IReportRepository reportRepository, 
        IUnitOfWork unitOfWork,
        ITaskReportGeneratorService taskReportGenerator)
    {
        _reportRepository = reportRepository;
        _unitOfWork = unitOfWork;
        _taskReportGenerator = taskReportGenerator;
    }

    /// <summary>
    /// Maneja el comando RequestTaskReportCommand y crea un reporte en estado REQUESTED
    /// </summary>
    public async Task<Report> HandleRequestTaskReportCommand(RequestTaskReportCommand command)
    {
        // Validar que se proporcionen las fechas
        var periodStart = command.PeriodStart ?? DateTime.UtcNow.AddMonths(-1);
        var periodEnd = command.PeriodEnd ?? DateTime.UtcNow;

        // Validar que se proporcione un PlotId
        if (command.PlotId == null)
        {
            throw new ArgumentException("PlotId es requerido para generar un reporte", nameof(command.PlotId));
        }

        // Crear el reporte usando el método factory Request
        var report = Report.Request(
            ReportType.ActivitySummary, // Tipo por defecto para reportes de tareas
            command.PlotId,
            command.RequestedBy,
            periodStart,
            periodEnd
        );

        // Guardar el reporte en estado REQUESTED
        await _reportRepository.AddAsync(report);
        await _unitOfWork.CompleteAsync();

        return report;
    }

    /// <summary>
    /// Maneja el comando GenerateReportCommand y genera el contenido del reporte
    /// </summary>
    public async Task<Report> HandleGenerateReportCommand(GenerateReportCommand command)
    {
        // Obtener el Report del repositorio
        var report = await _reportRepository.FindByIdAsync(command.ReportId);
        
        if (report == null)
        {
            throw new InvalidOperationException($"Report con ID {command.ReportId} no encontrado");
        }

        // Marcar el reporte como en proceso de generación
        report.MarkAsGenerating();
        await _unitOfWork.CompleteAsync();

        try
        {
            // Llamar al TaskReportGeneratorService para generar el JSON
            var jsonContent = await _taskReportGenerator.GenerateReportJsonAsync(report);

            // Convertir el JSON a byte[] usando UTF-8
            var contentBytes = Encoding.UTF8.GetBytes(jsonContent);

            // Guardar el contenido en report.content y cambiar status a GENERATED
            report.Generate(contentBytes);

            // Persistir los cambios
            await _unitOfWork.CompleteAsync();

            return report;
        }
        catch (Exception ex)
        {
            // Si hay un error, marcar el reporte como fallido
            report.MarkFailed($"Error al generar el reporte: {ex.Message}");
            await _unitOfWork.CompleteAsync();
            throw;
        }
    }
}

