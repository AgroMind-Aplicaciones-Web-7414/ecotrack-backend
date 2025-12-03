﻿namespace EcotrackPlatform.API.Report.Infrastructure.Services;

using System.Text.Json;
using EcotrackPlatform.API.Report.Domain.Model;
using EcotrackPlatform.API.Report.Domain.Services;
using EcotrackPlatform.API.Monitoringandcontrol.Domain.Repositories;
using EcotrackPlatform.API.Monitoringandcontrol.Domain.Model.ValueObjects;

/// <summary>
/// Implementación del servicio de generación de reportes de tareas
/// </summary>
public class TaskReportGeneratorService : ITaskReportGeneratorService
{
    private readonly ITaskRepository _taskRepository;

    public TaskReportGeneratorService(ITaskRepository taskRepository)
    {
        _taskRepository = taskRepository;
    }

    public async Task<string> GenerateReportJsonAsync(Report report)
    {
        // Obtener todas las tareas del repositorio
        var allTasks = await _taskRepository.GetAllAsync();

        // Filtrar las tareas por el período del reporte
        var tasksInPeriod = allTasks.Where(t =>
            t.CreatedAt >= report.PeriodStart && t.CreatedAt <= report.PeriodEnd
        ).ToList();

        // Calcular estadísticas
        var totalTasks = tasksInPeriod.Count;
        var completedTasks = tasksInPeriod.Count(t => t.Status == TaskStatus.Completed);
        var pendingTasks = tasksInPeriod.Count(t => t.Status == TaskStatus.Pending);
        var inProgressTasks = tasksInPeriod.Count(t => t.Status == TaskStatus.InProgress);

        // Crear la estructura del reporte con datos reales
        var reportData = new
        {
            ReportId = report.Id,
            Type = report.Type.ToString(),
            PlotId = report.PlotId.Value,
            RequestedBy = report.RequestedBy.Value,
            Period = new
            {
                Start = report.PeriodStart,
                End = report.PeriodEnd
            },
            GeneratedAt = DateTime.UtcNow,
            Summary = new
            {
                TotalTasks = totalTasks,
                CompletedTasks = completedTasks,
                PendingTasks = pendingTasks,
                InProgressTasks = inProgressTasks,
                CompletionRate = totalTasks > 0 ? Math.Round((double)completedTasks / totalTasks * 100, 2) : 0
            },
            Tasks = tasksInPeriod.Select(t => new
            {
                Id = t.Id,
                Title = t.Title,
                ResponsibleId = t.ResponsibleId,
                Status = t.Status.ToString(),
                StartedAt = t.StartedAt,
                CompletedAt = t.CompletedAt,
                CreatedAt = t.CreatedAt,
                UpdatedAt = t.UpdatedAt
            }).ToList()
        };

        var json = JsonSerializer.Serialize(reportData, new JsonSerializerOptions
        {
            WriteIndented = true,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        });

        return await Task.FromResult(json);
    }
}

