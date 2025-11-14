namespace EcotrackPlatform.API.Monitoringandcontrol.Domain.Model.Entities;

using System;

public class ChecklistItem
{
    public int Id { get; private set; }
    public int ChecklistId { get; private set; }
    public string Description { get; private set; }
    
    private ChecklistItem() { }

    public ChecklistItem(string description)
    {
        Description = description ?? throw  new ArgumentNullException(nameof(description));;
    }
}