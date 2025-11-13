using Microsoft.EntityFrameworkCore;

namespace EcotrackPlatform.API.Profile.Infrastructure.Persistence.EFC.Configuration.Extensions
{
    public static class ModelBuilderExtensions
    {
        // Mantengo este nombre porque tu AppDbContext lo invoca así:
        public static void AddProfileModule(this ModelBuilder modelBuilder)
        {
            // ---- Profile ----
            modelBuilder.Entity<EcotrackPlatform.API.Profile.Domain.Model.Aggregates.Profile>(b =>
            {
                b.ToTable("profiles");
                b.HasKey(p => p.Id);
                // Mapeo mínimo: sin navegar ni props adicionales para evitar CS1061
                // Cuando confirmemos propiedades reales (DisplayName, Email, etc.), las agregamos aquí.
            });

            // ---- ProfileSettings ----
            modelBuilder.Entity<EcotrackPlatform.API.Profile.Domain.Model.Aggregates.ProfileSettings>(b =>
            {
                b.ToTable("profile_settings");
                b.HasKey(s => s.Id);
                // Igual: mapeo mínimo; sin asumir Language/TimeZone ni navegación
                // Luego definimos la 1-1 con Profile si así lo deseas.
            });
        }
    }
}