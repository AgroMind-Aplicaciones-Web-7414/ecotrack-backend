namespace EcotrackPlatform.API.Monitoringandcontrol.Infraestructure.Persistence.EFC.Configuration;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using EcotrackPlatform.API.Monitoringandcontrol.Domain.Model.Entities;

public class ChecklistItemConfiguration :  IEntityTypeConfiguration<ChecklistItem>
{
    public void Configure(EntityTypeBuilder<ChecklistItem> builder)
    {
        builder.HasKey(c => c.Id);
        builder.Property(c => c.Id).ValueGeneratedOnAdd();
    }
}