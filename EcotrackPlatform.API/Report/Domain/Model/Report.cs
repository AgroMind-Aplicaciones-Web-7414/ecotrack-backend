﻿namespace EcotrackPlatform.API.Report.Domain.Model;

using EcotrackPlatform.API.Report.Domain.Model.ValueObjects;

public class Report
{
    public int Id { get; private set; }
    public ReportId ReportId { get; private set; }
    public ProfileId RequestedBy { get; private set; }
    public ReportStatus Status { get; private set; }
    public PlotId PlotId { get; private set; }
    public ReportType Type { get; private set; }
    public DateTime PeriodStart { get; private set; }
    public DateTime PeriodEnd { get; private set; }
    public DateTime? GeneratedAt { get; private set; }
    public byte[]? Content { get; private set; }
    public string? FailureReason { get; private set; }
    
    public DateTime CreatedAt { get; private set; }
    public DateTime? UpdatedAt { get; private set; }

    // Constructor privado para EF Core
    private Report()
    {
        ReportId = new ReportId();
        RequestedBy = new ProfileId();
        PlotId = new PlotId();
    }

    // Constructor privado para creación interna
    private Report(ReportType type, PlotId plotId, ProfileId requestedBy, DateTime periodStart, DateTime periodEnd)
    {
        Type = type;
        PlotId = plotId ?? throw new ArgumentNullException(nameof(plotId));
        RequestedBy = requestedBy ?? throw new ArgumentNullException(nameof(requestedBy));
        PeriodStart = periodStart;
        PeriodEnd = periodEnd;
        Status = ReportStatus.Requested;
        CreatedAt = DateTime.UtcNow;
        ReportId = new ReportId();
    }

    /// <summary>
    /// Método de fábrica para solicitar un nuevo reporte
    /// </summary>
    public static Report Request(ReportType type, PlotId plotId, ProfileId requestedBy, DateTime periodStart, DateTime periodEnd)
    {
        if (periodEnd < periodStart)
        {
            throw new ArgumentException("La fecha de fin no puede ser anterior a la fecha de inicio");
        }

        return new Report(type, plotId, requestedBy, periodStart, periodEnd);
    }

    /// <summary>
    /// Genera el reporte con los datos proporcionados
    /// </summary>
    public void Generate(byte[] data)
    {
        if (Status != ReportStatus.Requested && Status != ReportStatus.Generating)
        {
            throw new InvalidOperationException($"No se puede generar un reporte en estado {Status}");
        }

        if (data == null || data.Length == 0)
        {
            throw new ArgumentException("Los datos del reporte no pueden estar vacíos", nameof(data));
        }

        Content = data;
        Status = ReportStatus.Generated;
        GeneratedAt = DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Marca el reporte como fallido con una razón
    /// </summary>
    public void MarkFailed(string reason)
    {
        if (string.IsNullOrWhiteSpace(reason))
        {
            throw new ArgumentException("Debe proporcionar una razón para el fallo", nameof(reason));
        }

        Status = ReportStatus.Failed;
        FailureReason = reason;
        UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Marca el reporte como en proceso de generación
    /// </summary>
    public void MarkAsGenerating()
    {
        if (Status != ReportStatus.Requested)
        {
            throw new InvalidOperationException($"No se puede comenzar a generar un reporte en estado {Status}");
        }

        Status = ReportStatus.Generating;
        UpdatedAt = DateTime.UtcNow;
    }
}