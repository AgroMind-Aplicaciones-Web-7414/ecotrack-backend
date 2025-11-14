namespace EcotrackPlatform.API.Monitoringandcontrol.Application.Internal.QueryServices;

using EcotrackPlatform.API.Monitoringandcontrol.Domain.Model.Entities;
using EcotrackPlatform.API.Monitoringandcontrol.Domain.Repositories;
using System.Threading.Tasks;

public class GetChecklistByTaskIdQueryService
{
    private readonly IChecklistRepository _checklistRepository;

    public GetChecklistByTaskIdQueryService(IChecklistRepository checklistRepository)
    {
        _checklistRepository = checklistRepository;
    }

    public async Task<Checklist?> Handle(string taskId)
    {
        return await _checklistRepository.GetByTaskId(taskId);
    }
}