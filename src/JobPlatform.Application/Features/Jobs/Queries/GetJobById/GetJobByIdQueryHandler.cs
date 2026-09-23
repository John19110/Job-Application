using MediatR;
using JobPlatform.Application.DTOs.Jobs;
using JobPlatform.Application.Interfaces;

namespace JobPlatform.Application.Features.Jobs.Queries.GetJobById;

public class GetJobByIdQueryHandler : IRequestHandler<GetJobByIdQuery, JobResponseDto>
{
    private readonly IJobService _jobService;

    public GetJobByIdQueryHandler(IJobService jobService)
    {
        _jobService = jobService;
    }

    public async Task<JobResponseDto> Handle(GetJobByIdQuery request, CancellationToken cancellationToken)
    {
        return await _jobService.GetJobByIdAsync(request.Id, cancellationToken);
    }
}

