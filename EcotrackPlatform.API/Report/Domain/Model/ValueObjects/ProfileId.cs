﻿namespace EcotrackPlatform.API.Report.Domain.Model.ValueObjects;

public record ProfileId(int Value)
{
    public ProfileId() : this(0) { }
}

