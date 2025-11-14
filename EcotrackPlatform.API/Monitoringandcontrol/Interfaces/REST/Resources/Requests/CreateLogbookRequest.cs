namespace EcotrackPlatform.API.Monitoringandcontrol.Interfaces.REST.Resources.Requests;

public record CreateLogbookRequest(string Activity, DateTime Duration, int Volume,  string Evident);