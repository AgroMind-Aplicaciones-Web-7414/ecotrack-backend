using EcotrackPlatform.API.Shared.Infrastructure.Persistence.EFC.Configuration;

namespace EcotrackPlatform.API.Monitoringandcontrol.Infraestructure.Persistence.EFC.Respositories;

using Microsoft.EntityFrameworkCore;
using EcotrackPlatform.API.Monitoringandcontrol.Domain.Model.Aggregates;
using EcotrackPlatform.API.Monitoringandcontrol.Domain.Repositories;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

public class TaskRepository : ITaskRepository
{
    private readonly AppDbContext _dbContext;

    public TaskRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<int> AddAsync(TaskAggregate task)
    {
        await _dbContext.Set<TaskAggregate>().AddAsync(task);
        await _dbContext.SaveChangesAsync();
        return task.Id;
    }

    public async Task<TaskAggregate> GetByIdAsync(int id)
    {
       return await _dbContext.Set<TaskAggregate>().FindAsync(id); 
    }

    public async Task<List<TaskAggregate>> GetAllAsync()
    {
        return await _dbContext.Set<TaskAggregate>().ToListAsync();
    }

    public async Task UpdateAsync(TaskAggregate task)
    {
        task.SetUpdatedAt();
        _dbContext.Set<TaskAggregate>().Update(task);
        await _dbContext.SaveChangesAsync();
    }
}