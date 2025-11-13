namespace EcotrackPlatform.API.Monitoringandcontrol.Infraestructure.Persistence.EFC.Extensions;

using Microsoft.EntityFrameworkCore;
using EcotrackPlatform.API.Monitoringandcontrol.Infraestructure.Persistence.EFC.Configuration;

public static class ModelBuilderExtensions
{
   public static void ApplyConfigurations(this ModelBuilder modelBuilder)
   {
      modelBuilder.ApplyConfiguration(new TaskConfiguration());
      modelBuilder.ApplyConfiguration(new ChecklistConfiguration());
      modelBuilder.ApplyConfiguration(new ChecklistItemConfiguration());
      modelBuilder.ApplyConfiguration(new LogbookConfiguration());
   } 
}