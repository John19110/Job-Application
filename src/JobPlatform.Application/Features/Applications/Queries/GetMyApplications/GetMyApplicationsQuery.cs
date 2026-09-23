using MediatR;
using JobPlatform.Application.DTOs.Applications;

namespace JobPlatform.Application.Features.Applications.Queries.GetMyApplications;

public record GetMyApplicationsQuery() : IRequest<IReadOnlyList<JobApplicationResponseDto>>;

