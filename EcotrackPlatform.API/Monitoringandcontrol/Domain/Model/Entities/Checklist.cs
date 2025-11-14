namespace EcotrackPlatform.API.Monitoringandcontrol.Domain.Model.Entities;

using System;
using System.Collections.Generic;

public class Checklist
{
    public int Id { get; private set; }
    public string TaskId { get; private set; }
    public string Title { get; private set; }
    public List<ChecklistItem> Items { get; private set; }
    
    private Checklist(){}

    public Checklist(string taskId, string title)
    {
        TaskId = taskId;
        Title = title ?? throw new ArgumentNullException(nameof(title));
        Items = new List<ChecklistItem>();
    }

    public void AddItem(string description)
    {
        Items.Add(new ChecklistItem(description));
    }
}