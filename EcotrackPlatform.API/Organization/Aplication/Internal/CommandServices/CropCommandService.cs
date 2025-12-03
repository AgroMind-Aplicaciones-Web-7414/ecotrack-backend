using EcotrackPlatform.API.Organization.Aplication.Services;
using EcotrackPlatform.API.Organization.Domain.Model.Commands;
using EcotrackPlatform.API.Organization.Domain.Model.Aggregates;
using EcotrackPlatform.API.Organization.Domain.Repositories;
using EcotrackPlatform.API.Shared.Domain.Repositories;


namespace EcotrackPlatform.API.Organization.Aplication.Internal.CommandServices;

public class CropCommandService(
    ICropRepository cropRepository,
    IUnitOfWork unitOfWork) : ICropCommandService
{
    public async Task<Crop> Handle(CreateCropCommand command)
    {
        var crop = new Crop
        {
            Name = command.Name,
            Description = command.Description,
            OrganizationId = command.OrganizationId,
            CreatedAt = DateTime.UtcNow
        };

        await cropRepository.AddAsync(crop);
        await unitOfWork.CompleteAsync();

        return crop;
    }

    public async Task<bool> Handle(int id)
    {
        var crop = await cropRepository.FindByIdAsync(id);
        if (crop == null) return false;

        cropRepository.Remove(crop);
        await unitOfWork.CompleteAsync();
        return true;
    }
}