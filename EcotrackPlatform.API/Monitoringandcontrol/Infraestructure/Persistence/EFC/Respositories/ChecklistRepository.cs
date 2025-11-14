using EcotrackPlatform.API.Shared.Infrastructure.Persistence.EFC.Configuration;

namespace EcotrackPlatform.API.Monitoringandcontrol.Infraestructure.Persistence.EFC.Respositories;

using Microsoft.EntityFrameworkCore;
using EcotrackPlatform.API.Monitoringandcontrol.Domain.Model.Entities;
using EcotrackPlatform.API.Monitoringandcontrol.Domain.Repositories;
using System.Threading.Tasks;

public class ChecklistRepository : IChecklistRepository
{
    private readonly AppDbContext _dbContext;

    public ChecklistRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<int> AddAsync(Checklist checklist)
    {
        await _dbContext.Set<Checklist>().AddAsync(checklist);
        await _dbContext.SaveChangesAsync();
        return checklist.Id;
    }

    public async Task<Checklist?> GetByTaskId(string taskId)
    {
        return await _dbContext.Set<Checklist>().Include(c => c.Items).FirstOrDefaultAsync(x => x.TaskId == taskId);
    }
}