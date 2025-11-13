using EcotrackPlatform.API.Shared.Infrastructure.Persistence.EFC.Configuration.Extensions;
using EntityFrameworkCore.CreatedUpdatedDate.Extensions;
using Microsoft.EntityFrameworkCore;
using EcotrackPlatform.API.Monitoringandcontrol.Domain.Model.Aggregates;
using EcotrackPlatform.API.Monitoringandcontrol.Domain.Model.Entities;
using EcotrackPlatform.API.Monitoringandcontrol.Infraestructure.Persistence.EFC.Extensions;

namespace EcotrackPlatform.API.Shared.Infrastructure.Persistence.EFC.Configuration;

public class AppDbContext(DbContextOptions options) : DbContext(options)
{
    public DbSet<TaskAggregate> Tasks { get; set; }
    public DbSet<Checklist> Checklists { get; set; }
    public DbSet<ChecklistItem> ChecklistItems { get; set; }
    protected override void OnConfiguring(DbContextOptionsBuilder builder)
    {
        // Automatically set CreatedDate and UpdatedDate for entities
        builder.AddCreatedUpdatedInterceptor();
        base.OnConfiguring(builder);
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        
        // Apply naming convention to use snake_case for database objects
        builder.ApplyConfigurations();
        builder.Entity<TaskAggregate>().ToTable("Tasks");
        builder.Entity<Checklist>().ToTable("Checklists");
        builder.Entity<ChecklistItem>().ToTable("ChecklistItems");
    }
}