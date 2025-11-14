namespace EcotrackPlatform.API.Report.Infrastructure.Persistence.EFC.Configuration;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using EcotrackPlatform.API.Report.Domain.Model;
using EcotrackPlatform.API.Report.Domain.Model.ValueObjects;

public class ReportConfiguration : IEntityTypeConfiguration<Report>
{
    public void Configure(EntityTypeBuilder<Report> builder)
    {
        builder.ToTable("reports");
        
        builder.HasKey(r => r.Id);
        builder.Property(r => r.Id).HasColumnName("id").ValueGeneratedOnAdd();

        // Value Objects como propiedades simples con conversión
        builder.Property(r => r.ReportId)
            .HasColumnName("report_id")
            .HasConversion(
                v => v.Value,
                v => new ReportId(v));

        builder.Property(r => r.RequestedBy)
            .HasColumnName("requested_by")
            .HasConversion(
                v => v.Value,
                v => new ProfileId(v));

        builder.Property(r => r.PlotId)
            .HasColumnName("plot_id")
            .HasConversion(
                v => v.Value,
                v => new PlotId(v));

        // Enums
        builder.Property(r => r.Status)
            .HasColumnName("status")
            .HasConversion<string>();

        builder.Property(r => r.Type)
            .HasColumnName("type")
            .HasConversion<string>();

        // DateTime properties
        builder.Property(r => r.PeriodStart).HasColumnName("period_start");
        builder.Property(r => r.PeriodEnd).HasColumnName("period_end");
        builder.Property(r => r.GeneratedAt).HasColumnName("generated_at");
        builder.Property(r => r.CreatedAt).HasColumnName("created_at");
        builder.Property(r => r.UpdatedAt).HasColumnName("updated_at");

        // Content as byte array
        builder.Property(r => r.Content)
            .HasColumnName("content")
            .HasColumnType("bytea"); // PostgreSQL binary type

        // Failure reason
        builder.Property(r => r.FailureReason)
            .HasColumnName("failure_reason")
            .HasMaxLength(500);
    }
}

