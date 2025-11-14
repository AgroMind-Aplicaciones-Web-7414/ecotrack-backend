﻿namespace EcotrackPlatform.API.Report.Domain.Repositories;

using EcotrackPlatform.API.Report.Domain.Model;
using EcotrackPlatform.API.Shared.Domain.Repositories;

public interface IReportRepository : IBaseRepository<Report>
{
    Task<Report?> FindByIdAsync(int id);
    Task<IEnumerable<Report>> FindByRequestedByAsync(int profileId);
}

