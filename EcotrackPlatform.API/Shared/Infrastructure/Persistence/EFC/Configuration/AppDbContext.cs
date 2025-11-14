using EcotrackPlatform.API.Shared.Infrastructure.Persistence.EFC.Configuration.Extensions;
using EntityFrameworkCore.CreatedUpdatedDate.Extensions;
using Microsoft.EntityFrameworkCore;
using EcotrackPlatform.API.Profile.Infrastructure.Persistence.EFC.Configuration.Extensions; // AddProfileModule

namespace EcotrackPlatform.API.Shared.Infrastructure.Persistence.EFC.Configuration
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        // --- DbSets mínimos (ajusta con tus entidades reales) ---
        public DbSet<EcotrackPlatform.API.Profile.Domain.Model.Aggregates.Profile> Profiles => Set<EcotrackPlatform.API.Profile.Domain.Model.Aggregates.Profile>();
        public DbSet<EcotrackPlatform.API.Profile.Domain.Model.Aggregates.ProfileSettings> ProfileSettings => Set<EcotrackPlatform.API.Profile.Domain.Model.Aggregates.ProfileSettings>();

        // Si ya tienes AuthSession en Iam, puedes exponerlo:
        public DbSet<EcotrackPlatform.API.Iam.Domain.Model.Aggregates.AuthSession> AuthSessions => Set<EcotrackPlatform.API.Iam.Domain.Model.Aggregates.AuthSession>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Módulos por bounded context
            modelBuilder.AddProfileModule();

            // Si tienes más módulos/convenciones globales, agrégalas aquí:
            // modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
            // Model-wide defaults (opcional)
            // foreach (var prop in modelBuilder.Model.GetEntityTypes()
            //          .SelectMany(t => t.GetProperties())
            //          .Where(p => p.ClrType == typeof(string)))
            // {
            //     prop.SetMaxLength(255);
            // }
        }
    }
}