namespace EcotrackPlatform.API.Report.Infrastructure.Persistence.EFC.Repositories;

using Microsoft.EntityFrameworkCore;
using EcotrackPlatform.API.Report.Domain.Model;
using EcotrackPlatform.API.Report.Domain.Repositories;
using EcotrackPlatform.API.Shared.Infrastructure.Persistence.EFC.Configuration;
using EcotrackPlatform.API.Shared.Infrastructure.Persistence.Repositories;

public class ReportRepository : BaseRepository<Report>, IReportRepository
{
    public ReportRepository(AppDbContext context) : base(context)
    {
    }

    public new async Task<Report?> FindByIdAsync(int id)
    {
        return await Context.Set<Report>().FindAsync(id);
    }

    public async Task<IEnumerable<Report>> FindByRequestedByAsync(int profileId)
    {
        return await Context.Set<Report>()
            .Where(r => r.RequestedBy.Value == profileId)
            .OrderByDescending(r => r.CreatedAt)
            .ToListAsync();
    }
}

