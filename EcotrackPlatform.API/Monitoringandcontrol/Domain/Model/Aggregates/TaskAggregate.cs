namespace EcotrackPlatform.API.Monitoringandcontrol.Domain.Model.Aggregates;

using System;
using EcotrackPlatform.API.Monitoringandcontrol.Domain.Model.ValueObjects;

public class TaskAggregate
{
    public int Id { get; private set; }
    public string Title { get; private set; } = string.Empty;
    public string ResponsibleId  { get; private set; } =  string.Empty;
    public TaskStatus Status { get; private set; } = TaskStatus.Pending;
    public string StartedAt { get; private set; } = string.Empty;
    public string CompletedAt { get; private set; } = string.Empty;
    
    public DateTime CreatedAt { get; private set; }
    public DateTime? UpdatedAt { get; private set; }

    private TaskAggregate()
    {
    }

    public TaskAggregate(string title, string responsibleId)
    {
        Title = title ?? throw new ArgumentNullException(nameof(title));
        ResponsibleId = responsibleId;
        CreatedAt = DateTime.Now;
    }

    public void Start()
    {
        if (Status == TaskStatus.Pending)
        {
            Status = TaskStatus.InProgress;
            StartedAt = DateTime.Now.ToString("dd/MM/yyyy");
            UpdatedAt = DateTime.Now;
        }
    }

    public void Complete()
    {
        if (Status == TaskStatus.InProgress)
        {
            Status = TaskStatus.Completed;
            CompletedAt = DateTime.Now.ToString("dd/MM/yyyy");
            UpdatedAt = DateTime.Now;
        }
    }

    public void SetUpdatedAt()
    {
        UpdatedAt = DateTime.Now;
    }
}