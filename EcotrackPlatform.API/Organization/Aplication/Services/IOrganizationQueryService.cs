﻿using EcotrackPlatform.API.Organization.Domain.Model.Queries;

namespace EcotrackPlatform.API.Organization.Aplication.Services;

public interface IOrganizationQueryService
{
    Task<Domain.Model.Aggregates.Organization?> Handle(GetOrganizationByIdQuery query);
    Task<IEnumerable<Domain.Model.Aggregates.Organization>> Handle(GetAllOrganizationsQuery query);
}