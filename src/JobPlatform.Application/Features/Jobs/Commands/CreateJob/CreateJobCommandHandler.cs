using MediatR;
using JobPlatform.Application.DTOs.Jobs;
using JobPlatform.Application.Interfaces;

namespace JobPlatform.Application.Features.Jobs.Commands.CreateJob;

public class CreateJobCommandHandler : IRequestHandler<CreateJobCommand, JobResponseDto>
{
    private readonly IJobService _jobService;

    public CreateJobCommandHandler(IJobService jobService)
    {
        _jobService = jobService;
    }

    public async Task<JobResponseDto> Handle(CreateJobCommand request, CancellationToken cancellationToken)
    {
        var dto = new CreateJobRequestDto(request.Title, request.Description);
        return await _jobService.CreateJobAsync(dto, cancellationToken);
    }
}

