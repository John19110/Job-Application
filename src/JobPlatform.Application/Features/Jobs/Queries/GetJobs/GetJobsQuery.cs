using MediatR;
using JobPlatform.Application.DTOs.Jobs;

namespace JobPlatform.Application.Features.Jobs.Queries.GetJobs;

public record GetJobsQuery() : IRequest<IReadOnlyList<JobResponseDto>>;

