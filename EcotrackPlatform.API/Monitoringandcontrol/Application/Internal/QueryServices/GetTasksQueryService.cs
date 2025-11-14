namespace EcotrackPlatform.API.Monitoringandcontrol.Application.Internal.QueryServices;

using EcotrackPlatform.API.Monitoringandcontrol.Domain.Model.Aggregates;
using EcotrackPlatform.API.Monitoringandcontrol.Domain.Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;

public class GetTasksQueryService
{
    private readonly ITaskRepository _taskRepository;

    public GetTasksQueryService(ITaskRepository taskRepository)
    {
        _taskRepository = taskRepository;
    }

    public async Task<List<TaskAggregate>> Handle()
    {
        return await _taskRepository.GetAllAsync();
    }
}