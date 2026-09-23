using MediatR;
using JobPlatform.Application.DTOs.Jobs;

namespace JobPlatform.Application.Features.Jobs.Queries.GetJobById;

public record GetJobByIdQuery(Guid Id) : IRequest<JobResponseDto>;

