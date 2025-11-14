namespace EcotrackPlatform.API.Monitoringandcontrol.Interfaces.REST.Transform;

using EcotrackPlatform.API.Monitoringandcontrol.Interfaces.REST.Resources.Responses;
using EcotrackPlatform.API.Monitoringandcontrol.Domain.Model.Aggregates;

public class TaskAssembler
{
    public static TaskResource ToResource(TaskAggregate task)
    {
        return new TaskResource(
                task.Id.ToString(),
                task.Title,
                task.ResponsibleId,
                task.Status.ToString(),
                task.StartedAt,
                task.CompletedAt
            );
    }

    public static CreatedTaskResource ToCreatedResource(TaskAggregate task)
    {
        return new CreatedTaskResource(
                task.Id.ToString(),
                task.Title,
                task.ResponsibleId
            );
    }
}