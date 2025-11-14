namespace EcotrackPlatform.API.Report.Infrastructure.Persistence.EFC.Configuration.Extensions;

using Microsoft.EntityFrameworkCore;
using EcotrackPlatform.API.Report.Infrastructure.Persistence.EFC.Configuration;

public static class ModelBuilderExtensions
{
    public static void AddReportModule(this ModelBuilder builder)
    {
        builder.ApplyConfiguration(new ReportEntityTypeConfiguration());    }
}

