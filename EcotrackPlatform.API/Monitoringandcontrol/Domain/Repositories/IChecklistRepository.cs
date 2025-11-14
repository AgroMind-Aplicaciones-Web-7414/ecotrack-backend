namespace EcotrackPlatform.API.Monitoringandcontrol.Domain.Repositories;

using EcotrackPlatform.API.Monitoringandcontrol.Domain.Model.Entities;
using System.Threading.Tasks;

public interface IChecklistRepository
{
    Task<int> AddAsync(Checklist checklist);
    Task<Checklist?> GetByTaskId(string taskId);
}