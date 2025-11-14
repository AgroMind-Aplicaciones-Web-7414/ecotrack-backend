namespace EcotrackPlatform.API.Report.Interfaces.REST.Resources;

using System.Text.Json;

/// <summary>
/// Recurso de respuesta con los detalles del reporte
/// </summary>
public record ReportDetailResource(
    int Id,
    int ReportId,
    int RequestedBy,
    string Status,
    int PlotId,
    string Type,
    DateTime PeriodStart,
    DateTime PeriodEnd,
    DateTime? GeneratedAt,
    DateTime CreatedAt,
    DateTime? UpdatedAt,
    string? FailureReason,
    JsonDocument? Content
);

