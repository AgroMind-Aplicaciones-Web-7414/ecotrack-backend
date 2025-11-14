namespace EcotrackPlatform.API.Report.Application.Internal.QueryServices;

using System.Text;
using System.Text.Json;
using EcotrackPlatform.API.Report.Application.Internal.DTOs;
using EcotrackPlatform.API.Report.Domain.Repositories;

public class ReportQueryService
{
    private readonly IReportRepository _reportRepository;

    public ReportQueryService(IReportRepository reportRepository)
    {
        _reportRepository = reportRepository;
    }

    /// <summary>
    /// Obtiene un reporte por ID con su metadata y contenido decodificado como JSON
    /// </summary>
    public async Task<ReportDetailDto?> GetReportByIdAsync(int reportId)
    {
        var report = await _reportRepository.FindByIdAsync(reportId);
        
        if (report == null)
        {
            return null;
        }

        // Crear el DTO con la metadata del reporte
        var dto = new ReportDetailDto
        {
            Id = report.Id,
            ReportId = report.ReportId.Value,
            RequestedBy = report.RequestedBy.Value,
            Status = report.Status.ToString(),
            PlotId = report.PlotId.Value,
            Type = report.Type.ToString(),
            PeriodStart = report.PeriodStart,
            PeriodEnd = report.PeriodEnd,
            GeneratedAt = report.GeneratedAt,
            CreatedAt = report.CreatedAt,
            UpdatedAt = report.UpdatedAt,
            FailureReason = report.FailureReason
        };

        // Decodificar el contenido si existe
        if (report.Content != null && report.Content.Length > 0)
        {
            try
            {
                // Convertir byte[] a string usando UTF-8
                var jsonString = Encoding.UTF8.GetString((byte[])report.Content);
                
                // Parsear el JSON a JsonDocument
                dto.Content = JsonDocument.Parse(jsonString);
            }
            catch (JsonException)
            {
                // Si hay error al parsear, dejar Content como null
                dto.Content = null;
            }
        }

        return dto;
    }

    /// <summary>
    /// Obtiene todos los reportes de un perfil específico
    /// </summary>
    public async Task<IEnumerable<ReportDetailDto>> GetReportsByProfileIdAsync(int profileId)
    {
        var reports = await _reportRepository.FindByRequestedByAsync(profileId);
        var dtoList = new List<ReportDetailDto>();

        foreach (var report in reports)
        {
            var dto = new ReportDetailDto
            {
                Id = report.Id,
                ReportId = report.ReportId.Value,
                RequestedBy = report.RequestedBy.Value,
                Status = report.Status.ToString(),
                PlotId = report.PlotId.Value,
                Type = report.Type.ToString(),
                PeriodStart = report.PeriodStart,
                PeriodEnd = report.PeriodEnd,
                GeneratedAt = report.GeneratedAt,
                CreatedAt = report.CreatedAt,
                UpdatedAt = report.UpdatedAt,
                FailureReason = report.FailureReason
            };

            // Decodificar contenido si existe
            if (report.Content != null && report.Content.Length > 0)
            {
                try
                {
                    var jsonString = Encoding.UTF8.GetString(report.Content);
                    dto.Content = JsonDocument.Parse(jsonString);
                }
                catch (JsonException)
                {
                    dto.Content = null;
                }
            }

            dtoList.Add(dto);
        }

        return dtoList;
    }
}

