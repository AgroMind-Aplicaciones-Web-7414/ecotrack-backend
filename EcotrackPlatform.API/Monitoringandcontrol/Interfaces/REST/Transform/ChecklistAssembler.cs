using EcotrackPlatform.API.Monitoringandcontrol.Interfaces.REST.Resources.Responses;

namespace EcotrackPlatform.API.Monitoringandcontrol.Interfaces.REST.Transform;

using EcotrackPlatform.API.Monitoringandcontrol.Domain.Model.Entities;
using System.Linq;

public static class ChecklistAssembler
{
    public static ChecklistResource ToResource(Checklist checklist)
    {
        var items = checklist.Items.Select(i => new ChecklistItemResource(i.Id.ToString(), i.Description)).ToList();
        return new ChecklistResource(
            checklist.Id.ToString(),
            checklist.TaskId,
            checklist.Title,
            items
        );
    }
}