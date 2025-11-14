using EcotrackPlatform.API.Organization.Domain.Model.Aggregates;
using EcotrackPlatform.API.Organization.Domain.Model.Commands;

namespace EcotrackPlatform.API.Organization.Aplication.Services;

public interface ICropCommandService
{
    Task<Crop> Handle(CreateCropCommand command);
}