namespace EcotrackPlatform.API.Monitoringandcontrol.Interfaces.REST.Resources.Requests;

public record CreateChecklistRequest(string TaskId, string Title, List<ChecklistItemRequest> Items);