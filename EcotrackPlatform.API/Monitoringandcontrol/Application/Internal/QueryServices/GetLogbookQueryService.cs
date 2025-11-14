using EcotrackPlatform.API.Monitoringandcontrol.Domain.Model.Aggregates;
using EcotrackPlatform.API.Monitoringandcontrol.Domain.Repositories;

namespace EcotrackPlatform.API.Monitoringandcontrol.Application.Internal.QueryServices;

public class GetLogbookQueryService
{
    private readonly ILogbookRepository _logbookRepository;

    public GetLogbookQueryService(ILogbookRepository logbookRepository)
    {
        _logbookRepository = logbookRepository;
    }

    public async Task<Logbook> Handle(int id)
    {
        return await _logbookRepository.GetByIdAsync(id);
    }

    public async Task<List<Logbook>> Handle()
    {
        return await _logbookRepository.GetAllAsync();
    }
}