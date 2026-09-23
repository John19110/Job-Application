using MediatR;
using JobPlatform.Application.DTOs.Applications;

namespace JobPlatform.Application.Features.Applications.Commands.ApplyForJob;

public record ApplyForJobCommand(Guid JobId) : IRequest<JobApplicationResponseDto>;

