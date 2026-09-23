using MediatR;
using JobPlatform.Application.DTOs.Jobs;
using JobPlatform.Application.Interfaces;

namespace JobPlatform.Application.Features.Jobs.Queries.GetJobs;

public class GetJobsQueryHandler : IRequestHandler<GetJobsQuery, IReadOnlyList<JobResponseDto>>
{
    private readonly IJobService _jobService;

    public GetJobsQueryHandler(IJobService jobService)
    {
        _jobService = jobService;
    }

    public async Task<IReadOnlyList<JobResponseDto>> Handle(GetJobsQuery request, CancellationToken cancellationToken)
    {
        return await _jobService.GetAllJobsAsync(cancellationToken);
    }
}

