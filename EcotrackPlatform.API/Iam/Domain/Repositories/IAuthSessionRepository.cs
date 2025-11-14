using EcotrackPlatform.API.Iam.Domain.Model.Aggregates;

namespace EcotrackPlatform.API.Iam.Domain.Repositories;

public interface IAuthSessionRepository
{
    Task<AuthSession?> FindByIdAsync(Guid id);
    Task AddAsync(AuthSession session);
    void Update(AuthSession session);
}