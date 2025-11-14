namespace EcotrackPlatform.API.Monitoringandcontrol.Application.Internal.CommandServices;

using EcotrackPlatform.API.Monitoringandcontrol.Domain.Model.Entities;
using EcotrackPlatform.API.Monitoringandcontrol.Domain.Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;

public class CreateChecklistCommandService
{
    private readonly IChecklistRepository _checklistRepository;

    public CreateChecklistCommandService(IChecklistRepository checklistRepository)
    {
        _checklistRepository = checklistRepository;
    }

    public async Task<int> Handle(string taskId, string title, List<string> items)
    {
        var checklist = new Checklist(taskId, title);
        foreach (var desc in items)
        {
            checklist.AddItem(desc);
        }
        return await _checklistRepository.AddAsync(checklist);
    }
}