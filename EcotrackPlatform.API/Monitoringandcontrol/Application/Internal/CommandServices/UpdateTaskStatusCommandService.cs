namespace EcotrackPlatform.API.Monitoringandcontrol.Application.Internal.CommandServices;

using EcotrackPlatform.API.Monitoringandcontrol.Domain.Repositories;
using System.Threading.Tasks;

public class UpdateTaskStatusCommandService
{
    private readonly ITaskRepository _taskRepository;
    private readonly IChecklistRepository _checklistRepository;

    public UpdateTaskStatusCommandService(ITaskRepository taskRepository, IChecklistRepository checklistRepository)
    {
        _taskRepository = taskRepository;
        _checklistRepository = checklistRepository;
    }

    public async Task Handle(int taskId, string status)
    {
        var task = await _taskRepository.GetByIdAsync(taskId);
        if (task == null)
        {
            throw new KeyNotFoundException($"Task with id {taskId} does not exist");
        }

        if (status == "InProgress")
        {
            task.Start();
        }
        else if (status == "Completed")
        {
            task.Complete();
        }
        
        await _taskRepository.UpdateAsync(task);
    }
}