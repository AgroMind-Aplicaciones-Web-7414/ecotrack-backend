using EcotrackPlatform.API.Monitoringandcontrol.Domain.Model.Aggregates;
using EcotrackPlatform.API.Monitoringandcontrol.Interfaces.REST.Resources.Responses;

namespace EcotrackPlatform.API.Monitoringandcontrol.Interfaces.REST.Transform;

public class LogbookAssembler
{
    public static LogbookResource ToResource(Logbook logbook)
    {
        return new LogbookResource(
            logbook.Id.ToString(),
            logbook.Activity,
            logbook.Duration.ToString(),
            logbook.Volume.ToString(),
            logbook.Evident
            );
    }
}