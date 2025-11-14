﻿namespace EcotrackPlatform.API.Report.Domain.Model.ValueObjects;

public record ReportId(int Value)
{
    public ReportId() : this(0) { }
}

