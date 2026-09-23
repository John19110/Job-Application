using MediatR;
using JobPlatform.Application.DTOs.Applications;
using JobPlatform.Application.Interfaces;

namespace JobPlatform.Application.Features.Applications.Commands.ApplyForJob;

public class ApplyForJobCommandHandler : IRequestHandler<ApplyForJobCommand, JobApplicationResponseDto>
{
    private readonly IApplicationService _applicationService;

    public ApplyForJobCommandHandler(IApplicationService applicationService)
    {
        _applicationService = applicationService;
    }

    public async Task<JobApplicationResponseDto> Handle(ApplyForJobCommand request, CancellationToken cancellationToken)
    {
        return await _applicationService.ApplyAsync(request.JobId, cancellationToken);
    }
}

