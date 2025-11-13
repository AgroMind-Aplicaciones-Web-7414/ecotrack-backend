namespace EcotrackPlatform.API.Monitoringandcontrol.Application.Internal.CommandServices;

using EcotrackPlatform.API.Monitoringandcontrol.Domain.Model.Aggregates;
using EcotrackPlatform.API.Monitoringandcontrol.Domain.Repositories;
using System.Threading.Tasks;

public class CreateTaskCommandService
{
    private readonly ITaskRepository _taskRepository;

    public CreateTaskCommandService(ITaskRepository taskRepository)
    {
        _taskRepository = taskRepository;
    }

    public async Task<int> Handle(string title, string responsibleId)
    {
        var task = new TaskAggregate(title, responsibleId);
        return await _taskRepository.AddAsync(task);
    }
}