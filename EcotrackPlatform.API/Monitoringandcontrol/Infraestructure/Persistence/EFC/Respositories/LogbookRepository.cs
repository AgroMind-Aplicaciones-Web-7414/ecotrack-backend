using EcotrackPlatform.API.Monitoringandcontrol.Domain.Model.Aggregates;
using EcotrackPlatform.API.Monitoringandcontrol.Domain.Repositories;
using EcotrackPlatform.API.Shared.Infrastructure.Persistence.EFC.Configuration;
using Microsoft.EntityFrameworkCore;

namespace EcotrackPlatform.API.Monitoringandcontrol.Infraestructure.Persistence.EFC.Respositories;

public class LogbookRepository : ILogbookRepository
{
    private readonly AppDbContext _dbContext;

    public LogbookRepository(AppDbContext dbContext)
    {
        _dbContext =  dbContext;
    }

    public async Task<int> AddAsync(Logbook logbook)
    {
        await _dbContext.Set<Logbook>().AddAsync(logbook);
        await _dbContext.SaveChangesAsync();
        return logbook.Id;
    }

    public async Task<Logbook> GetByIdAsync(int id)
    {
        return await _dbContext.Set<Logbook>().FindAsync(id);
    }

    public async Task<List<Logbook>> GetAllAsync()
    {
        return await _dbContext.Set<Logbook>().ToListAsync();
    }

    public async Task UpdateAsync(Logbook logbook)
    {
        _dbContext.Set<Logbook>().Update(logbook);
        await _dbContext.SaveChangesAsync();
    }
}