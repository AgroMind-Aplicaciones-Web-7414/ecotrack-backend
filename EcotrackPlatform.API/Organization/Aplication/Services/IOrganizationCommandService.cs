﻿using EcotrackPlatform.API.Organization.Domain.Model.Commands;

namespace EcotrackPlatform.API.Organization.Aplication.Services;

public interface IOrganizationCommandService
{
    Task<Domain.Model.Aggregates.Organization> Handle(CreateOrganizationCommand command);
    Task<bool> Handle(int id);
}