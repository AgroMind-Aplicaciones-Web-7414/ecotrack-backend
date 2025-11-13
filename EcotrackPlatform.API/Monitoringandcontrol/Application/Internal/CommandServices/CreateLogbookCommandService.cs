using EcotrackPlatform.API.Monitoringandcontrol.Domain.Model.Aggregates;
using EcotrackPlatform.API.Monitoringandcontrol.Domain.Repositories;

namespace EcotrackPlatform.API.Monitoringandcontrol.Application.Internal.CommandServices;

public class CreateLogbookCommandService
{
    private readonly ILogbookRepository _createLogbookRepository;

    public CreateLogbookCommandService(ILogbookRepository createLogbookRepository)
    {
        _createLogbookRepository = createLogbookRepository;
    }

    public async Task<int> Handle(string activity, DateTime duration, int volume, string evident)
    {
        var logbook = new Logbook(activity, duration, volume, evident);
        return await _createLogbookRepository.AddAsync(logbook);
    }
}