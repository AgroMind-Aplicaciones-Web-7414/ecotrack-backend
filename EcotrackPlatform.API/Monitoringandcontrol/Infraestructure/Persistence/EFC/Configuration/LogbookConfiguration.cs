

namespace EcotrackPlatform.API.Monitoringandcontrol.Infraestructure.Persistence.EFC.Configuration;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using EcotrackPlatform.API.Monitoringandcontrol.Domain.Model.Aggregates;

public class LogbookConfiguration : IEntityTypeConfiguration<Logbook>
{
    public void Configure(EntityTypeBuilder<Logbook> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedOnAdd();
        builder.Property(x => x.Activity).HasMaxLength(150).IsRequired();
        builder.Property(x => x.Duration).HasColumnType("date").IsRequired();
        builder.Property(x => x.Volume).IsRequired();
        builder.Property(x => x.Evident).IsRequired(); 
        builder.Property(x => x.ParcelId).IsRequired();
    }
}