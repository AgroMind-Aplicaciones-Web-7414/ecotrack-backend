using EcotrackPlatform.API.Organization.Domain.Model.Queries;
using EcotrackPlatform.API.Organization.Domain.Model.Aggregates;

namespace EcotrackPlatform.API.Organization.Aplication.Services;

public interface ICropQueryService
{
    Task<IEnumerable<Crop>> Handle(GetAllCropsByOrganizationIdQuery query);
}