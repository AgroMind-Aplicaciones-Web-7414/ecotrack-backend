namespace EcotrackPlatform.API.Monitoringandcontrol.Domain.Repositories;

using EcotrackPlatform.API.Monitoringandcontrol.Domain.Model.Aggregates;
using System.Collections.Generic;
using System.Threading.Tasks;

public interface ITaskRepository
{
    Task<int> AddAsync(TaskAggregate task);
    Task<TaskAggregate> GetByIdAsync(int id);
    Task<List<TaskAggregate>> GetAllAsync();
    Task UpdateAsync(TaskAggregate task);
}