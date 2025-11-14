﻿using EntityFrameworkCore.CreatedUpdatedDate.Extensions;
using Microsoft.EntityFrameworkCore;
using EcotrackPlatform.API.Profile.Infrastructure.Persistence.EFC.Configuration.Extensions;
using EcotrackPlatform.API.Shared.Infrastructure.Persistence.EFC.Configuration.Extensions;

using EcotrackPlatform.API.Monitoringandcontrol.Domain.Model.Aggregates;
using EcotrackPlatform.API.Monitoringandcontrol.Domain.Model.Entities;
using MonitoringExtensions = EcotrackPlatform.API.Monitoringandcontrol.Infraestructure.Persistence.EFC.Extensions.ModelBuilderExtensions;
using EcotrackPlatform.API.Report.Infrastructure.Persistence.EFC.Configuration.Extensions;

namespace EcotrackPlatform.API.Shared.Infrastructure.Persistence.EFC.Configuration;

public class AppDbContext(DbContextOptions options) : DbContext(options)
{
    public DbSet<TaskAggregate> Tasks { get; set; }
    public DbSet<Checklist> Checklists { get; set; }
    public DbSet<ChecklistItem> ChecklistItems { get; set; }
    public DbSet<Logbook> Logbooks { get; set; }
    
    // Report Module
    public DbSet<EcotrackPlatform.API.Report.Domain.Model.Report> Reports { get; set; }
    
    protected override void OnConfiguring(DbContextOptionsBuilder builder)
    {
        // Automatically set CreatedDate and UpdatedDate for entities
        builder.AddCreatedUpdatedInterceptor();
        base.OnConfiguring(builder);
    }

        // --- DbSets mínimos (ajusta con tus entidades reales) ---
        public DbSet<EcotrackPlatform.API.Profile.Domain.Model.Aggregates.Profile> Profiles => Set<EcotrackPlatform.API.Profile.Domain.Model.Aggregates.Profile>();
        public DbSet<EcotrackPlatform.API.Profile.Domain.Model.Aggregates.ProfileSettings> ProfileSettings => Set<EcotrackPlatform.API.Profile.Domain.Model.Aggregates.ProfileSettings>();

        // Si ya tienes AuthSession en Iam, puedes exponerlo:
        public DbSet<EcotrackPlatform.API.Iam.Domain.Model.Aggregates.AuthSession> AuthSessions => Set<EcotrackPlatform.API.Iam.Domain.Model.Aggregates.AuthSession>();

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Módulos por bounded context
            builder.AddProfileModule();
            
            // Aplicar configuraciones del módulo Report
            builder.AddReportModule();
            
            // Aplicar configuraciones del módulo Monitoringandcontrol
            MonitoringExtensions.ApplyConfigurations(builder);
            
            // Apply naming convention to use snake_case for database objects
            builder.UseSnakeCaseNamingConvention();
        }
}
