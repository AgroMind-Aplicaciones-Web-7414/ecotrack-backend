using EcotrackPlatform.API.Organization.Aplication.Services;
using EcotrackPlatform.API.Organization.Domain.Model.Commands;
using EcotrackPlatform.API.Organization.Domain.Repositories;
using EcotrackPlatform.API.Shared.Domain.Repositories;

namespace EcotrackPlatform.API.Organization.Aplication.Internal.CommandServices;

public class OrganizationCommandService(IOrganizationRepository repository, IUnitOfWork unitOfWork)
    : IOrganizationCommandService
{
    public async Task<Domain.Model.Aggregates.Organization> Handle(CreateOrganizationCommand command)
    {
        var org = new Domain.Model.Aggregates.Organization(
            command.Name,
            command.Description,
            command.Status
        );

        await repository.AddAsync(org);
        await unitOfWork.CompleteAsync();

        return org;
    }

    public async Task<bool> Handle(int id)
    {
        var organization = await repository.FindByIdAsync(id);
        if (organization == null) return false;

        repository.Remove(organization);
        await unitOfWork.CompleteAsync();
        return true;
    }
}