using EcotrackPlatform.API.Monitoringandcontrol.Domain.Model.Aggregates;

namespace EcotrackPlatform.API.Monitoringandcontrol.Domain.Repositories;

public interface ILogbookRepository
{
    Task<int> AddAsync(Logbook logbook);
    Task<Logbook> GetByIdAsync(int id);
    Task<List<Logbook>> GetAllAsync();
    Task UpdateAsync(Logbook logbook);
}