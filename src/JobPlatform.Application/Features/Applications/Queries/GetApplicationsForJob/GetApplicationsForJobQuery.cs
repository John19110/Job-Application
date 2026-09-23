using MediatR;
using JobPlatform.Application.DTOs.Applications;

namespace JobPlatform.Application.Features.Applications.Queries.GetApplicationsForJob;

public record GetApplicationsForJobQuery(Guid JobId) : IRequest<IReadOnlyList<JobApplicationResponseDto>>;

