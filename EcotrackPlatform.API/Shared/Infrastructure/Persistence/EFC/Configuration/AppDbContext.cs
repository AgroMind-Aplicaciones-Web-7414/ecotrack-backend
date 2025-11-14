using EcotrackPlatform.API.Organization.Domain.Model.Aggregates;
using EcotrackPlatform.API.Shared.Infrastructure.Persistence.EFC.Configuration.Extensions;
using EntityFrameworkCore.CreatedUpdatedDate.Extensions;
using Microsoft.EntityFrameworkCore;

namespace EcotrackPlatform.API.Shared.Infrastructure.Persistence.EFC.Configuration;

public class AppDbContext(DbContextOptions options) : DbContext(options)
{
    public DbSet<Organization.Domain.Model.Aggregates.Organization> Organizations { get; set; }
    public DbSet<Crop> Crops { get; set; }

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
        builder.UseSnakeCaseNamingConvention();
    }
}