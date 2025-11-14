namespace EcotrackPlatform.API.Monitoringandcontrol.Interfaces.REST.Resources.Responses;

public record TaskResource(string Id, string Title, string ResponsibleId, string Status, string StartedAt, string CompletedAt);