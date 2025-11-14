namespace EcotrackPlatform.API.Report.Domain.Commands;

using EcotrackPlatform.API.Report.Domain.Model.ValueObjects;

public record RequestTaskReportCommand(
    ProfileId RequestedBy,
    PlotId? PlotId,
    DateTime? PeriodStart,
    DateTime? PeriodEnd
);
