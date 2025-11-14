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
}