using EcotrackPlatform.API.Iam.Domain.Model.Aggregates;
using EcotrackPlatform.API.Iam.Domain.Repositories;
using EcotrackPlatform.API.Shared.Infrastructure.Persistence.EFC.Configuration;
using EcotrackPlatform.API.Shared.Infrastructure.Persistence.EFC.Configuration.Extensions;
using Microsoft.EntityFrameworkCore;

namespace EcotrackPlatform.API.Iam.Infrastructure.Repositories;

public class AuthSessionRepository : IAuthSessionRepository
{
    private readonly AppDbContext _ctx;
    public AuthSessionRepository(AppDbContext ctx) { _ctx = ctx; }

    public async Task<AuthSession?> FindByIdAsync(Guid id) =>
        await _ctx.AuthSessions.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);

    public async Task AddAsync(AuthSession session) => await _ctx.AuthSessions.AddAsync(session);

    public void Update(AuthSession session) => _ctx.AuthSessions.Update(session);
}