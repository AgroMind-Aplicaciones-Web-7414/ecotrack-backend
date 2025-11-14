namespace EcotrackPlatform.API.Monitoringandcontrol.Interfaces.REST.Resources.Responses;

public record ChecklistResource(string Id, string TaskId, string Title, List<ChecklistItemResource> Items);