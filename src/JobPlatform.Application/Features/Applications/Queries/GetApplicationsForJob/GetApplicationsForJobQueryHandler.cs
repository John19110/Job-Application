using MediatR;
using JobPlatform.Application.DTOs.Applications;
using JobPlatform.Application.Interfaces;

namespace JobPlatform.Application.Features.Applications.Queries.GetApplicationsForJob;

public class GetApplicationsForJobQueryHandler : IRequestHandler<GetApplicationsForJobQuery, IReadOnlyList<JobApplicationResponseDto>>
{
    private readonly IApplicationService _applicationService;

    public GetApplicationsForJobQueryHandler(IApplicationService applicationService)
    {
        _applicationService = applicationService;
    }

    public async Task<IReadOnlyList<JobApplicationResponseDto>> Handle(GetApplicationsForJobQuery request, CancellationToken cancellationToken)
    {
        return await _applicationService.GetApplicationsForJobAsync(request.JobId, cancellationToken);
    }
}

