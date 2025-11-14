﻿namespace EcotrackPlatform.API.Report.Domain.Model.ValueObjects;

public record PlotId(int Value)
{
    public PlotId() : this(0) { }
}

