namespace EcotrackPlatform.API.Report.Interfaces.REST.Transform;

using EcotrackPlatform.API.Report.Domain.Commands;
using EcotrackPlatform.API.Report.Domain.Model.ValueObjects;
using EcotrackPlatform.API.Report.Interfaces.REST.Resources;

public static class RequestTaskReportCommandFromResourceAssembler
{
    public static RequestTaskReportCommand ToCommand(RequestTaskReportResource resource)
    {
        return new RequestTaskReportCommand(
            new ProfileId(resource.RequestedBy),
            resource.PlotId.HasValue ? new PlotId(resource.PlotId.Value) : null,
            resource.PeriodStart,
            resource.PeriodEnd
        );
    }
}

