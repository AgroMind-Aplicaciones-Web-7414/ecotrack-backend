namespace EcotrackPlatform.API.Shared.Domain.Repositories;

public interface IUnitOfWork
{
    Task CompleteAsync();
}