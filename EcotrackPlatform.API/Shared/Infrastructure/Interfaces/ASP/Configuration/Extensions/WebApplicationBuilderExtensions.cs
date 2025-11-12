using EcotrackPlatform.API.Shared.Domain.Repositories;
using EcotrackPlatform.API.Shared.Infrastructure.Persistence.Repositories;

namespace EcotrackPlatform.API.Shared.Infrastructure.Interfaces.ASP.Configuration.Extensions;

public static class WebApplicationBuilderExtensions
{
    public static void AddSharedContextServices(this WebApplicationBuilder builder)
    {
        // Profiles Bounded Context Dependency Injection Configuration
        builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
    }
}