namespace EcotrackPlatform.API.Monitoringandcontrol.Infraestructure.Persistence.EFC.Configuration;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using EcotrackPlatform.API.Monitoringandcontrol.Domain.Model.Aggregates;

public class TaskConfiguration :  IEntityTypeConfiguration<TaskAggregate>
{
    public void Configure(EntityTypeBuilder<TaskAggregate> builder)
    {
        builder.HasKey(c => c.Id);
        builder.Property(c => c.Id).ValueGeneratedOnAdd();
        builder.Property(c => c.Title).HasMaxLength(150).IsRequired();
        builder.Property(c => c.ResponsibleId).IsRequired();
        builder.Property(c => c.Status).HasConversion<string>();
        builder.Property(c => c.StartedAt).HasMaxLength(10);
        builder.Property(c => c.CompletedAt).HasMaxLength(10);
        builder.Property(c => c.CreatedAt).HasColumnType("datetime").HasDefaultValueSql("CURRENT_TIMESTAMP");
        builder.Property(c => c.UpdatedAt).HasColumnType("datetime").IsRequired(false);
    }
}