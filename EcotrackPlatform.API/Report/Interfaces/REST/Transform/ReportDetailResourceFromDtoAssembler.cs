namespace EcotrackPlatform.API.Report.Interfaces.REST.Transform;

using EcotrackPlatform.API.Report.Application.Internal.DTOs;
using EcotrackPlatform.API.Report.Interfaces.REST.Resources;

public static class ReportDetailResourceFromDtoAssembler
{
    public static ReportDetailResource ToResource(ReportDetailDto dto)
    {
        return new ReportDetailResource(
            dto.Id,
            dto.ReportId,
            dto.RequestedBy,
            dto.Status,
            dto.PlotId,
            dto.Type,
            dto.PeriodStart,
            dto.PeriodEnd,
            dto.GeneratedAt,
            dto.CreatedAt,
            dto.UpdatedAt,
            dto.FailureReason,
            dto.Content
        );
    }
}

/// <summary>
/// Recurso para solicitar la generación de un reporte de tareas
/// </summary>
public record RequestTaskReportResource(
    int RequestedBy,
    int? PlotId,
    DateTime? PeriodStart,
    DateTime? PeriodEnd
);

