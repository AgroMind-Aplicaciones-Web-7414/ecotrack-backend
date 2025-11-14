namespace EcotrackPlatform.API.Report.Application.Internal.DTOs;

using System.Text.Json;

/// <summary>
/// DTO que contiene la metadata del reporte y el contenido decodificado como JSON
/// </summary>
public class ReportDetailDto
{
    public int Id { get; set; }
    public int ReportId { get; set; }
    public int RequestedBy { get; set; }
    public string Status { get; set; } = string.Empty;
    public int PlotId { get; set; }
    public string Type { get; set; } = string.Empty;
    public DateTime PeriodStart { get; set; }
    public DateTime PeriodEnd { get; set; }
    public DateTime? GeneratedAt { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public string? FailureReason { get; set; }
    
    /// <summary>
    /// Contenido del reporte decodificado como JsonDocument
    /// </summary>
    public JsonDocument? Content { get; set; }
}

