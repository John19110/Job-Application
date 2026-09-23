using MediatR;
using JobPlatform.Application.DTOs.Jobs;

namespace JobPlatform.Application.Features.Jobs.Commands.CreateJob;

public record CreateJobCommand(string Title, string Description) : IRequest<JobResponseDto>;

