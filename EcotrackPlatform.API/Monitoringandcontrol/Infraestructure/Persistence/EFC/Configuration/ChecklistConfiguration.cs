namespace EcotrackPlatform.API.Monitoringandcontrol.Infraestructure.Persistence.EFC.Configuration;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using EcotrackPlatform.API.Monitoringandcontrol.Domain.Model.Entities;

public class ChecklistConfiguration :  IEntityTypeConfiguration<Checklist>
{
    public void Configure(EntityTypeBuilder<Checklist> builder)
    {
        builder.HasKey(c => c.Id);
        builder.Property(c => c.Id).ValueGeneratedOnAdd();
        builder.Property(c => c.TaskId).IsRequired();
        builder.Property(c => c.Title).HasMaxLength(150).IsRequired();
        builder.HasMany(c => c.Items).WithOne().HasForeignKey(c => c.ChecklistId).OnDelete(DeleteBehavior.Cascade);
    }
}